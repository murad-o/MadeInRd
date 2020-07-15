using ExporterWeb.Areas.Identity.Authorization;
using ExporterWeb.Helpers;
using ExporterWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ExporterWeb.Pages.Exporters
{
    public class CreateModel : BasePageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public CreateModel(ApplicationDbContext context, IAuthorizationService authorizationService,
            UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
            AuthorizationService = authorizationService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Init(_userManager);
            // TODO: authorize and add Where(exporters => exporters.UserId == current_user_id)
            var exporters = _context.CommonExporters.Include(nameof(CommonExporter.User));
            if (!IsAdminOrManager)
                exporters = exporters.Where(e => e.UserId == UserId);

            var items = await exporters.ToListAsync();
            ViewData["CommonExporterList"] = new SelectList(items, "UserId", "User.Email");
            ViewData["CommonExporterFirstId"] = items.FirstOrDefault()?.UserId;
            ViewData["WhiteListLanguages"] = new SelectList(Languages.WhiteList);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || !Languages.WhiteList.Contains(LanguageExporter.Language))
                return Page();

            if (!await IsAuthorized(LanguageExporter, AuthorizationOperations.Create))
                return Forbid();

            var languageExporter = new LanguageExporter();
            if (await TryUpdateModelAsync(
                    languageExporter,
                    nameof(LanguageExporter),
                    l => l.CommonExporterId, l => l.Language, l => l.Name, l => l.Description,
                    l => l.ContactPersonFirstName, l => l.ContactPersonSecondName, l => l.ContactPersonPatronymic,
                    l => l.DirectorFirstName, l => l.DirectorSecondName, l => l.DirectorPatronymic,
                    l => l.WorkingTime, l => l.Address, l => l.Website, l => l.Approved))
            {
                // If the user is a regular person, mark it as pending
                Init(_userManager);
                if (!IsAdminOrManager)
                    languageExporter.Approved = false;

                await _context.LanguageExporters!.AddAsync(languageExporter);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Details",
                    new { id = LanguageExporter.CommonExporterId, language = LanguageExporter.Language });
            }

            return Page();
        }

#nullable disable
        [BindProperty]
        public LanguageExporter LanguageExporter { get; set; }
    }
}
