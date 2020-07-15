using ExporterWeb.Areas.Identity.Authorization;
using ExporterWeb.Helpers;
using ExporterWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ExporterWeb.Pages.Products
{
    public class EditModel : BasePageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<EditModel> _logger;

        public EditModel(ApplicationDbContext context, UserManager<User> userManager,
            ILogger<EditModel> logger, IAuthorizationService authorizationService)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
            AuthorizationService = authorizationService;
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
            if (!await IsAuthorized(Product, AuthorizationOperations.Update))
            {
                _logger.LogInformation($"User {UserId} tries to edit product {Product.Id}");
                return Forbid();
            }

            ViewData["FieldOfActivityId"] = new SelectList(_context.FieldsOfActivity, "Id", "Name");
            ViewData["WhiteListLanguages"] = new SelectList(Languages.WhiteList);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
                return Page();

            if (!await IsAuthorized(Product, AuthorizationOperations.Update))
            {
                string message = $"User {UserId} tries to edit product {Product.Id}";
                _logger.LogWarning(message);
                return Forbid();
            }

            var product = await _context.Products!.FindAsync(id);
            if (await TryUpdateModelAsync(
                    product,
                    nameof(Product),
                    p => p.Name, p => p.Description,
                    p => p.Language, p => p.FieldOfActivityId))
            {
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }
            return Page();
        }

#nullable disable
        [BindProperty]
        public Product Product { get; set; }
    }
}
