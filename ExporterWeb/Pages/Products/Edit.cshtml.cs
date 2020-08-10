using ExporterWeb.Areas.Identity.Authorization;
using ExporterWeb.Helpers;
using ExporterWeb.Helpers.Services;
using ExporterWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ExporterWeb.Pages.Products
{
    [ValidateModel]
    public class EditModel : BasePageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EditModel> _logger;
        private readonly ImageService _imageService;

        public EditModel(ApplicationDbContext context,
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
                .Include(p => p.LanguageExporter)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Product is null)
                return NotFound();

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
            if (!await IsAuthorized(Product, AuthorizationOperations.Update))
            {
                string message = $"User {UserId} tries to edit product {Product.Id}";
                _logger.LogWarning(message);
                return Forbid();
            }

            var product = await _context.Products!.FindAsync(id);
            var oldLogo = product.Logo;

            if (Logo is { })
                product.Logo = _imageService.Save(ImageTypes.ProductLogo, Logo);

            if (await TryUpdateModelAsync(
                    product,
                    nameof(Product),
                    p => p.Name, p => p.Description,
                    p => p.Language, p => p.FieldOfActivityId))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    if (oldLogo is { } && Logo is { })
                        _imageService.Delete(ImageTypes.ProductLogo, oldLogo);
                }
                catch
                {
                    if (Logo is { })
                        _imageService.Delete(ImageTypes.ProductLogo, product.Logo!);
                }

                return RedirectToPage("./Index");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteImage(int id)
        {
            Product product = await _context.Products!.FindAsync(id);
            _imageService.Delete(ImageTypes.ProductLogo, product.Logo!);
            product.Logo = null;
            await _context.SaveChangesAsync();
            return StatusCode(StatusCodes.Status204NoContent);
        }

#nullable disable
        [BindProperty]
        public Product Product { get; set; }

        [BindProperty]
        public IFormFile Logo { get; set; }
    }
}
