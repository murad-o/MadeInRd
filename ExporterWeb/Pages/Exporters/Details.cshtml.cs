using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ExporterWeb.Models;
using Microsoft.AspNetCore.Authorization;

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
