using ExporterWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ExporterWeb.Pages.Admin.FieldsOfActivity
{
    public class EditModel : PageModel
    {
        private readonly ExporterWeb.Models.ApplicationDbContext _context;

        public EditModel(ExporterWeb.Models.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public FieldOfActivity? FieldOfActivity { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            FieldOfActivity = await _context.FieldsOfActivity.FirstOrDefaultAsync(m => m.Id == id);

            if (FieldOfActivity == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(FieldOfActivity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FieldOfActivityExists(FieldOfActivity!.Id))
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

        private bool FieldOfActivityExists(int id)
        {
            return _context.FieldsOfActivity.Any(e => e.Id == id);
        }
    }
}
