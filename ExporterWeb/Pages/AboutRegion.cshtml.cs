using System.Globalization;
using System.Threading.Tasks;
using ExporterWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ExporterWeb.Pages
{
    public class AboutRegion : PageModel
    {
        private readonly ApplicationDbContext _context;

        public AboutRegion(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task OnGetAsync()
        {
            AboutRegionModel = await _context.AboutRegionContents!
                .FirstOrDefaultAsync(model => model.Lang == CultureInfo.CurrentCulture.TwoLetterISOLanguageName);
        }
        
        #nullable disable
        [BindProperty]
        public AboutRegionModel AboutRegionModel { get; set; }
    }
}