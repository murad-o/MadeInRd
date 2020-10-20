using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ExporterWeb.Models;

namespace ExporterWeb.Pages.Admin.AboutRegion
{
    public class DeleteModel : PageModel
    {
        private readonly ExporterWeb.Models.ApplicationDbContext _context;

        public DeleteModel(ExporterWeb.Models.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public AboutRegionModel AboutRegionModel { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AboutRegionModel = await _context.AboutRegionContents.FirstOrDefaultAsync(m => m.Lang == id);

            if (AboutRegionModel == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AboutRegionModel = await _context.AboutRegionContents!.FindAsync(id);

            if (AboutRegionModel != null)
            {
                _context.AboutRegionContents.Remove(AboutRegionModel);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
