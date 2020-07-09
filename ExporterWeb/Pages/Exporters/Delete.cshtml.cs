using ExporterWeb.Areas.Identity.Authorization;
using ExporterWeb.Helpers;
using ExporterWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ExporterWeb.Pages.Exporters
{
    public class DeleteModel : BasePageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EditModel> _logger;

        public DeleteModel(ApplicationDbContext context, IAuthorizationService authorizationService,
            ILogger<EditModel> logger)
        {
            _context = context;
            _logger = logger;
            AuthorizationService = authorizationService;
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id is null)
                return NotFound();

            LanguageExporter = await _context.LanguageExporters
                .Include(l => l.CommonExporter)
                .FirstOrDefaultAsync(m => m.CommonExporterId == id && m.Language == Language);

            if (LanguageExporter is null)
                return NotFound();

            if (!await IsAuthorized(LanguageExporter, AuthorizationOperations.Delete))
            {
                _logger.LogInformation($"User {UserId} tries to delete exporter {id} ({Language})");
                return Forbid();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var id = LanguageExporter.CommonExporterId;
            if (id is null || !Languages.WhiteList.Contains(LanguageExporter.Language))
                return NotFound();

            LanguageExporter = await _context.LanguageExporters!.FindAsync(id, LanguageExporter.Language);

            if (LanguageExporter is null)
                return NotFound();

            if (!await IsAuthorized(LanguageExporter, AuthorizationOperations.Delete))
            {
                _logger.LogInformation($"User {UserId} failed to delete exporter {id} ({Language})");
                return Forbid();
            }

            _context.LanguageExporters.Remove(LanguageExporter);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index", new { Language });
        }

#nullable disable
        [BindProperty]
        public LanguageExporter LanguageExporter { get; set; }
    }
}
