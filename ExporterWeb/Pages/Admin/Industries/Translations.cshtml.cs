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

        #nullable disable
        public Industry Industry { get; set; }
    }
}