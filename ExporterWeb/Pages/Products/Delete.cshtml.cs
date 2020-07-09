using ExporterWeb.Areas.Identity.Authorization;
using ExporterWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ExporterWeb.Pages.Products
{
    public class DeleteModel : BasePageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<EditModel> _logger;

        public DeleteModel(ApplicationDbContext context, UserManager<User> userManager,
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
                .Include(p => p.LanguageExporter).FirstOrDefaultAsync(m => m.Id == id);

            if (Product is null)
                return NotFound();

            Init(_userManager);
            if (!await IsAuthorized(Product, AuthorizationOperations.Delete))
            {
                _logger.LogInformation($"User {UserId} tries to delete product {Product.Id}");
                return Forbid();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id is null)
                return NotFound();

            Product = await _context.Products!.FindAsync(id);
            if (Product is null)
                return NotFound();

            Init(_userManager);
            if (!await IsAuthorized(Product!, AuthorizationOperations.Delete))
            {
                string message = $"User {UserId} tries to delete product {Product.Id}";
                _logger.LogWarning(message);
                return Forbid();
            }

            _context.Products.Remove(Product);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

#nullable disable
        [BindProperty]
        public Product Product { get; set; }
    }
}
