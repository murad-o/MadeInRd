using ExporterWeb.Areas.Identity.Authorization;
using ExporterWeb.Helpers;
using ExporterWeb.Helpers.Services;
using ExporterWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ExporterWeb.Pages.Exporters
{
    [ValidateModel]
    public class EditModel : BasePageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EditModel> _logger;
        private readonly ImageService _imageService;

        public EditModel(ApplicationDbContext context,
            IAuthorizationService authorizationService,
            ILogger<EditModel> logger,
            UserManager<User> userManager,
            ImageService imageService)
        {
            _context = context;
            _logger = logger;
            _imageService = imageService;
            UserManager = userManager;
            AuthorizationService = authorizationService;
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id is null || Language is null)
                return NotFound();

            LanguageExporter = await _context.LanguageExporters
                .Include(l => l.CommonExporter)
                .FirstOrDefaultAsync(m => m.CommonExporterId == id && m.Language == Language);

            if (LanguageExporter is null)
                return NotFound();

            if (await IsAuthorized(LanguageExporter, AuthorizationOperations.Update))
                return Page();
            
            _logger.LogInformation($"User {UserId} tries to edit exporter {id} ({Language})");
            return Forbid();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (!Languages.WhiteList.Contains(LanguageExporter.Language))
                return Page();

            if (!await IsAuthorized(LanguageExporter, AuthorizationOperations.Update))
                return Forbid();

            var languageExporter = await _context.LanguageExporters
                .FirstOrDefaultAsync(l => l.CommonExporterId == id && l.Language == LanguageExporter.Language);

            var oldLogo = languageExporter.Logo;
            if (Logo is { })
                languageExporter.Logo = _imageService.Save(ImageTypes.ExporterLogo, Logo);

            if (!await TryUpdateModelAsync(
                languageExporter,
                nameof(LanguageExporter),
                l => l.Name, l => l.Description, l => l.ContactPersonFirstName,
                l => l.ContactPersonSecondName, l => l.ContactPersonPatronymic,
                l => l.DirectorFirstName, l => l.DirectorSecondName, l => l.DirectorPatronymic,
                l => l.WorkingTime, l => l.Address, l => l.Website))
                return Page();

            try
            {
                await _context.SaveChangesAsync();
                if (oldLogo is { } && Logo is { })
                    _imageService.Delete(ImageTypes.ExporterLogo, oldLogo);
            }
            catch
            {
                if (Logo is { })
                    _imageService.Delete(ImageTypes.ExporterLogo, languageExporter.Logo!);
            }
            return RedirectToPage("./Details", new { id, language = Language });
        }

        public async Task<IActionResult> OnPostDeleteImage(string id, string language)
        {
            var exporter = await _context.LanguageExporters!
                .FirstOrDefaultAsync(exp => exp.CommonExporterId == id && exp.Language == language);

            _imageService.Delete(ImageTypes.ExporterLogo, exporter.Logo!);
            exporter.Logo = null;
            await _context.SaveChangesAsync();
            return StatusCode(StatusCodes.Status204NoContent);
        }

#nullable disable
        [BindProperty]
        public LanguageExporter LanguageExporter { get; set; }

        [BindProperty]
        public IFormFile Logo { get; set; }
    }
}
