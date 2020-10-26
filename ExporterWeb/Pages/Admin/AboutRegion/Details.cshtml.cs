using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ExporterWeb.Models;

namespace ExporterWeb.Pages.Admin.AboutRegion
{
    public class DetailsModel : PageModel
    {
        private readonly ExporterWeb.Models.ApplicationDbContext _context;

        public DetailsModel(ExporterWeb.Models.ApplicationDbContext context)
        {
            _context = context;
        }

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

#nullable disable
        public AboutRegionModel AboutRegionModel { get; set; }
    }
}
