using System.Threading.Tasks;
using ExporterWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ExporterWeb.Pages.Industries
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }
            
            IndustryTranslation = await _context.IndustryTranslations!.FirstOrDefaultAsync(i => i.Id == id);

            if (IndustryTranslation is null)
            {
                return NotFound();
            } 

            return Page();
        }
        
        #nullable disable
        public IndustryTranslation IndustryTranslation { get; set; }
    }
}