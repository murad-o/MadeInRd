using ExporterWeb.Areas.Identity.Authorization;
using ExporterWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ExporterWeb.Pages.Admin.FieldsOfActivity
{
    [Authorize(Roles = Constants.AdministratorsRole + ", " + Constants.ManagersRole)]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            FieldOfActivity = await _context.FieldsOfActivity.FirstOrDefaultAsync(m => m.Id == id);

            if (FieldOfActivity == null)
                return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
                return NotFound();

            FieldOfActivity = await _context.FieldsOfActivity!.FindAsync(id);

            if (FieldOfActivity != null)
            {
                _context.FieldsOfActivity.Remove(FieldOfActivity);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }

#nullable disable
        [BindProperty]
        public FieldOfActivity FieldOfActivity { get; set; }
    }
}
