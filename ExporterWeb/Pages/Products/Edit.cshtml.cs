using ExporterWeb.Areas.Identity.Authorization;
using ExporterWeb.Helpers;
using ExporterWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace ExporterWeb.Pages.Products
{
    public class EditModel : BasePageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<EditModel> _logger;
        private readonly IAuthorizationService _authorizationService;

        public EditModel(ApplicationDbContext context,
            UserManager<User> userManager,
            ILogger<EditModel> logger,
            IAuthorizationService authorizationService)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
            _authorizationService = authorizationService;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id is null)
                return NotFound();

            Product = await _context.Products
                .Include(p => p.FieldOfActivity)
                .Include(p => p.LanguageExporter)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Product is null)
                return NotFound();

            Init(_userManager);
            if (!await IsAuthorized(Product))
            {
                _logger.LogInformation($"User {UserId} tries to edit product {Product.Id}");
                return Forbid();
            }

            if (IsAdminOrManager)
            {
                var exporters = await _context.CommonExporters
                    .Include(nameof(CommonExporter.User))
                    .ToListAsync();
                ViewData["CommonExporterList"] = new SelectList(exporters, "UserId", "User.Email");
            }
            ViewData["FieldOfActivityId"] = new SelectList(_context.FieldsOfActivity, "Id", "Name");
            ViewData["WhiteListLanguages"] = new SelectList(Languages.WhiteList);

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();
            
            Init(_userManager);

            if (!IsAdminOrManager)
            {
                var origin = await _context.Products!.FirstAsync(p => p.Id == Product.Id);
                Product.LanguageExporterId = origin.LanguageExporterId;
                _context.Entry(origin).State = EntityState.Detached;
            }

            if (!await IsAuthorized(Product))
            {
                string message = $"User {UserId} tries to edit product {Product.Id}";
                _logger.LogWarning(message);
                return Forbid();
            }
            _context.Attach(Product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(Product.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }

        private async Task<bool> IsAuthorized(Product product)
        {
            var result = await _authorizationService.AuthorizeAsync(
                User, product, AuthorizationOperations.Update);
            return result.Succeeded;
        }

#nullable disable
        [BindProperty]
        public Product Product { get; set; }
    }
}
