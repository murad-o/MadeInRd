using ExporterWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ExporterWeb.Pages.Admin.FieldsOfActivity
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public FieldOfActivity? FieldOfActivity { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            FieldOfActivity = await _context.FieldsOfActivity.FirstOrDefaultAsync(m => m.Id == id);

            if (FieldOfActivity == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
