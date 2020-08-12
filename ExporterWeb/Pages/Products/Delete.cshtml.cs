using ExporterWeb.Areas.Identity.Authorization;
using ExporterWeb.Helpers;
using ExporterWeb.Helpers.Services;
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
        private readonly ILogger<EditModel> _logger;
        private readonly ImageService _imageService;

        public DeleteModel(ApplicationDbContext context,
            UserManager<User> userManager,
            ILogger<EditModel> logger,
            IAuthorizationService authorizationService,
            ImageService imageService)
        {
            _context = context;
            UserManager = userManager;
            _logger = logger;
            _imageService = imageService;
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

            if (await IsAuthorized(Product, AuthorizationOperations.Delete))
                return Page();
            
            _logger.LogInformation($"User {UserId} tries to delete product {Product.Id}");
            return Forbid();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id is null)
                return NotFound();

            Product = await _context.Products!.FindAsync(id);
            if (Product is null)
                return NotFound();

            if (!await IsAuthorized(Product!, AuthorizationOperations.Delete))
            {
                string message = $"User {UserId} tries to delete product {Product.Id}";
                _logger.LogWarning(message);
                return Forbid();
            }

            _context.Products.Remove(Product);
            await _context.SaveChangesAsync();

            if (Product.Logo is { })
                _imageService.Delete(ImageTypes.ProductLogo, Product.Logo);

            return RedirectToPage("./Index");
        }

#nullable disable
        [BindProperty]
        public Product Product { get; set; }
    }
}
