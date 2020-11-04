using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ExporterWeb.Areas.Identity.Authorization;
using ExporterWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ExporterWeb.Pages.Admin.Industries
{
    [Authorize(Roles = Constants.AdministratorsRole)]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task OnGetAsync()
        {
            Industries =  await _context.IndustryTranslations!.Include(i => i.Industry).Where(i =>
                i.Language == Language).ToListAsync();
        }
        
        public async Task<IActionResult> OnPostDeleteIndustryAsync(int id)
        {
            var industry = await _context.Industries!.FirstOrDefaultAsync(i => i.Id == id);
            
            if (industry is null)
            {
                return NotFound();
            }
            
            _context.Remove(industry);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
        
        #nullable disable
        public IList<IndustryTranslation> Industries { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Language { get; set; } = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
    }
}