using System.Threading.Tasks;
using ExporterWeb.Helpers;
using ExporterWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ExporterWeb.Pages.Admin.Industries
{
    [ValidateModel]
    public class Create : PageModel
    {
        private readonly ApplicationDbContext _context;

        public Create(ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            
        }
        
        public async Task<IActionResult> OnPostAsync()
        {
            Industry industry = new Industry();
            industry.Translations!.Add(IndustryTranslation);
            await _context.Industries!.AddAsync(industry);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }

#nullable disable
        [BindProperty]
        public IndustryTranslation IndustryTranslation { get; set; }
    }
}