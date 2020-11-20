using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ExporterWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ExporterWeb.Pages.Industries
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            IndustryTranslations = await _context.IndustryTranslations!.Where(i =>
                i.Language == CultureInfo.CurrentCulture.TwoLetterISOLanguageName).ToListAsync();

            return Page();
        }

        #nullable disable
        public IEnumerable<IndustryTranslation> IndustryTranslations { get; set; }
    }
}