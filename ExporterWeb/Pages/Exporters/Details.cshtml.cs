using ExporterWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ExporterWeb.Pages.Exporters
{
    public class DetailsModel : BasePageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            UserManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id is null)
                return NotFound();

            IQueryable<LanguageExporter> exporters = _context.LanguageExporters
                .Include(e => e.CommonExporter)
                .Include(e => e.CommonExporter!.FieldOfActivity);

            if (!IsAdminOrManager)
                exporters = exporters.Where(p => p.Approved || p.CommonExporterId == UserId);

            LanguageExporter = await exporters
                .FirstOrDefaultAsync(m => m.CommonExporterId == id && m.Language == Language);

            if (LanguageExporter is null)
                return NotFound();

            return Page();
        }

#nullable disable
        public LanguageExporter LanguageExporter { get; set; }
    }
}
