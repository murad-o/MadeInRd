using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExporterWeb.Areas.Identity.Authorization;
using ExporterWeb.Helpers;
using ExporterWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IActionResult> OnGetAsync()
        {
            SecondaryLanguages = Languages.WhiteList;
            SecondaryLanguages.Remove(Languages.DefaultLanguage);

            IndustryTranslations =  await _context.IndustryTranslations!
                .Include(i => i.Industry)
                .ThenInclude(i => i!.Translations)
                .Where(i =>
                i.Language == Languages.DefaultLanguage).ToListAsync();

            return Page();
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
        public IList<IndustryTranslation> IndustryTranslations { get; set; }

        public HashSet<string> SecondaryLanguages { get; private set; }
    }
}