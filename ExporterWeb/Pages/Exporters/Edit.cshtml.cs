using ExporterWeb.Areas.Identity.Authorization;
using ExporterWeb.Helpers;
using ExporterWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ExporterWeb.Pages.Exporters
{
    public class EditModel : BasePageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EditModel> _logger;

        public EditModel(ApplicationDbContext context, IAuthorizationService authorizationService,
            ILogger<EditModel> logger, UserManager<User> userManager)
        {
            _context = context;
            _logger = logger;
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

            if (!await IsAuthorized(LanguageExporter, AuthorizationOperations.Update))
            {
                _logger.LogInformation($"User {UserId} tries to edit exporter {id} ({Language})");
                return Forbid();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (!ModelState.IsValid || !Languages.WhiteList.Contains(LanguageExporter.Language))
                return Page();

            if (!await IsAuthorized(LanguageExporter, AuthorizationOperations.Update))
                return Forbid();

            var languageExporter = await _context.LanguageExporters
                .FirstOrDefaultAsync(l => l.CommonExporterId == id && l.Language == LanguageExporter.Language);

            if (await TryUpdateModelAsync(
                    languageExporter,
                    nameof(LanguageExporter),
                    l => l.Name, l => l.Description, l => l.ContactPersonFirstName,
                    l => l.ContactPersonSecondName, l => l.ContactPersonPatronymic,
                    l => l.DirectorFirstName, l => l.DirectorSecondName, l => l.DirectorPatronymic,
                    l => l.WorkingTime, l => l.Address, l => l.Website, l => l.Approved))
            {
                // If the user is a regular person, mark it as pending
                if (!IsAdminOrManager)
                    languageExporter.Approved = false;

                await _context.SaveChangesAsync();
                return RedirectToPage("./Details", new { id, language = Language });
            }
            return Page();
        }

#nullable disable
        [BindProperty]
        public LanguageExporter LanguageExporter { get; set; }
    }
}
