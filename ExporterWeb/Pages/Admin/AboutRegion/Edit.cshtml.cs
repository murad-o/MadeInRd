using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ExporterWeb.Models;

namespace ExporterWeb.Pages.Admin.AboutRegion
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            AboutRegionModel = await _context.AboutRegionContents!.FirstOrDefaultAsync(m => m.Lang == id);

            if (AboutRegionModel == null)
            {
                return NotFound();
            }
            return Page();
        }
        
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(AboutRegionModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AboutRegionModelExists(AboutRegionModel.Lang))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool AboutRegionModelExists(string id)
        {
            return _context.AboutRegionContents!.Any(e => e.Lang == id);
        }
        
        #nullable disable
        [BindProperty]
        public AboutRegionModel AboutRegionModel { get; set; }
    }
}
