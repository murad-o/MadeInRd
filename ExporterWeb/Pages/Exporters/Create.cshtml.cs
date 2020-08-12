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
using System.Linq;
using System.Threading.Tasks;

namespace ExporterWeb.Pages.Exporters
{
    [ValidateModel]
    public class CreateModel : BasePageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ImageService _imageService;

        public CreateModel(ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<User> userManager,
            ImageService imageService)
        {
            _context = context;
            _imageService = imageService;
            AuthorizationService = authorizationService;
            UserManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var exporters = _context.CommonExporters.Include(nameof(CommonExporter.User));
            if (!IsAdminOrManager)
                exporters = exporters.Where(e => e.UserId == UserId);

            var items = await exporters.ToListAsync();
            ViewData["CommonExporterList"] = new SelectList(items, "UserId", "User.Email");
            ViewData["WhiteListLanguages"] = new SelectList(Languages.WhiteList);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!Languages.WhiteList.Contains(LanguageExporter.Language))
                return Page();

            if (!await IsAuthorized(LanguageExporter, AuthorizationOperations.Create))
                return Forbid();

            var languageExporter = new LanguageExporter();

            if (Logo is { })
                languageExporter.Logo = _imageService.Save(ImageTypes.ExporterLogo, Logo);

            if (!await TryUpdateModelAsync(
                languageExporter,
                nameof(LanguageExporter),
                l => l.CommonExporterId, l => l.Language, l => l.Name, l => l.Description,
                l => l.ContactPersonFirstName, l => l.ContactPersonSecondName, l => l.ContactPersonPatronymic,
                l => l.DirectorFirstName, l => l.DirectorSecondName, l => l.DirectorPatronymic,
                l => l.WorkingTime, l => l.Address, l => l.Website, l => l.Approved))
                return Page();
            
            // If the user is a regular person, mark it as pending
            if (!IsAdminOrManager)
                languageExporter.Approved = false;

            await _context.LanguageExporters!.AddAsync(languageExporter);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                if (Logo is { })
                    _imageService.Delete(ImageTypes.ExporterLogo, languageExporter.Logo!);
            }
            return RedirectToPage("./Details",
                new { id = LanguageExporter.CommonExporterId, language = LanguageExporter.Language });

        }

#nullable disable
        [BindProperty]
        public LanguageExporter LanguageExporter { get; set; }

        [BindProperty]
        public IFormFile Logo { get; set; }
    }
}
