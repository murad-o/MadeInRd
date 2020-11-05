using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExporterWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ExporterWeb.Pages.Admin.Industries
{
    public class Translations : PageModel
    {
        private readonly ApplicationDbContext _context;

        public Translations(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGet(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            Industry = await _context.Industries!.Include(i => i.Translations)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (Industry is null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteTranslationAsync(int id)
        {
            var industryTranslation = await _context.IndustryTranslations!
                .Include(i => i.Industry)
                .ThenInclude(i => i.Translations)
                .FirstOrDefaultAsync(i => i.Id == id);
            
            if (industryTranslation is null)
            {
                return NotFound();
            }
        
            if (industryTranslation.Industry!.Translations!.Count == 1)
            {
                _context.Industries!.Remove(industryTranslation.Industry);
            }
            else
            {
                _context.IndustryTranslations!.Remove(industryTranslation);
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("./Translations", new{ Id = industryTranslation.IndustryId });
        }

        #nullable disable
        public Industry Industry { get; set; }
    }
}