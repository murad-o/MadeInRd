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
using System.Linq;
using System.Threading.Tasks;

namespace ExporterWeb.Pages.Products
{
    [ValidateModel]
    public class CreateModel : BasePageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CreateModel> _logger;
        private readonly ImageService _imageService;

        public CreateModel(ApplicationDbContext context,
            UserManager<User> userManager,
            ILogger<CreateModel> logger,
            IAuthorizationService authorizationService,
            ImageService imageService)
        {
            _context = context;
            UserManager = userManager;
            _logger = logger;
            _imageService = imageService;
            AuthorizationService = authorizationService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var exporters = _context.CommonExporters.Include(nameof(CommonExporter.User));
            if (!IsAdminOrManager)
                exporters = exporters.Where(e => e.UserId == UserId);

            var items = await exporters.ToListAsync();
            ViewData["CommonExporterList"] = new SelectList(items, "UserId", "User.Email");
            ViewData["WhiteListLanguages"] = new SelectList(Languages.WhiteList);
            var localizedFieldsOfActivity = await _context.FieldsOfActivity
                .Select(f => new { f.Id, Name = f.Name[Language!] })
                .ToListAsync();
            ViewData["FieldOfActivityId"] = new SelectList(localizedFieldsOfActivity, "Id", "Name");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!await IsAuthorized(Product, AuthorizationOperations.Create))
            {
                string message = $"User {UserId} tries to create product with LanguageExporterId {Product.LanguageExporterId}";
                _logger.LogWarning(message);
                return Forbid();
            }

            var product = new Product();

            if (Logo is { })
                product.Logo = _imageService.Save(ImageTypes.ProductLogo, Logo);

            if (await TryUpdateModelAsync(
                    product,
                    nameof(Product),
                    p => p.LanguageExporterId, p => p.Name,
                    p => p.Description, p => p.Language,
                    p => p.FieldOfActivityId))
            {
                await _context.Products!.AddAsync(product);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch
                {
                    if (Logo is { })
                        _imageService.Delete(ImageTypes.ProductLogo, product.Logo!);
                    throw;
                }
                return RedirectToPage("./Index");
            }
            return Page();
        }

#nullable disable
        [BindProperty]
        public Product Product { get; set; }

        [BindProperty]
        public IFormFile Logo { get; set; }
    }
}
