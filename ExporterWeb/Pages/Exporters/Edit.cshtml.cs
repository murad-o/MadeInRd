using ExporterWeb.Areas.Identity.Authorization;
using ExporterWeb.Helpers;
using ExporterWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace ExporterWeb.Pages.Exporters
{
    public class EditModel : BasePageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EditModel> _logger;
        private readonly UserManager<User> _userManager;

        public EditModel(ApplicationDbContext context, IAuthorizationService authorizationService,
            ILogger<EditModel> logger, UserManager<User> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
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

            ViewData["CommonExporterId"] = new SelectList(_context.CommonExporters, "UserId", "UserId");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            Init(_userManager);
            if (!ModelState.IsValid || !Languages.WhiteList.Contains(LanguageExporter.Language))
                return Page();

            string id = LanguageExporter.CommonExporterId;
            if (!await IsAuthorized(LanguageExporter, AuthorizationOperations.Update))
            {
                _logger.LogInformation($"User {UserId} failed to edit exporter {id} ({Language})");
                return Forbid();
            }

            // If the user is a regular person, mark it as pending
            if (!IsAdminOrManager)
                LanguageExporter.Approved = false;

            _context.Attach(LanguageExporter).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LanguageExporterExists(id))
                    return NotFound();
                else
                    throw;
            }

            return RedirectToPage("./Details", new { id, Language });
        }

        private bool LanguageExporterExists(string id) =>
            _context.LanguageExporters.Any(e => e.CommonExporterId == id);

#nullable disable
        [BindProperty]
        public LanguageExporter LanguageExporter { get; set; }
    }
}
