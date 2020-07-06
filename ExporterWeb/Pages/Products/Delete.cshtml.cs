using ExporterWeb.Areas.Identity.Authorization;
using ExporterWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ExporterWeb.Pages.Products
{
    public class DeleteModel : BasePageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IAuthorizationService _authorizationService;
        private readonly ILogger<EditModel> _logger;

        public DeleteModel(ApplicationDbContext context,
            UserManager<User> userManager,
            IAuthorizationService authorizationService,
            ILogger<EditModel> logger)
        {
            _context = context;
            _userManager = userManager;
            _authorizationService = authorizationService;
            _logger = logger;
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
            if (!await IsAuthorized(Product))
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
            if (!await IsAuthorized(Product!))
            {
                string message = $"User {UserId} tries to delete product {Product.Id}";
                _logger.LogWarning(message);
                return Forbid();
            }

            _context.Products.Remove(Product);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        private async Task<bool> IsAuthorized(Product product)
        {
            var result = await _authorizationService.AuthorizeAsync(
                User, product, AuthorizationOperations.Delete);
            return result.Succeeded;
        }

#nullable disable
        [BindProperty]
        public Product Product { get; set; }
    }
}
