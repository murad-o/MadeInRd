using ExporterWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ExporterWeb.Pages.Exporters
{
    [AllowAnonymous]
    public class DetailsModel : BasePageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id is null)
                return NotFound();

            LanguageExporter = await _context.LanguageExporters
                .Include(e => e.CommonExporter)
                .Include(e => e.CommonExporter!.FieldOfActivity)
                .FirstOrDefaultAsync(m => m.CommonExporterId == id && m.Language == Language);

            if (LanguageExporter is null)
                return NotFound();

            return Page();
        }

#nullable disable
        public LanguageExporter LanguageExporter { get; set; }
    }
}
