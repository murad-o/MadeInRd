using ExporterWeb.Areas.Identity.Authorization;
using ExporterWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ExporterWeb.Pages.Events
{
    [Authorize(Roles = Constants.AdministratorsRole + ", " + Constants.ManagersRole)]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id is null)
                return NotFound();

            Event = await _context.Events.FirstOrDefaultAsync(m => m.Id == id);

            if (Event is null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
                return Page();

            var eventToUpdate = await _context.Events!.FindAsync(id);

            if (eventToUpdate is null) 
                return NotFound();

            if (await TryUpdateModelAsync(
                    eventToUpdate,
                    "Event",
                    e => e.Name, e => e.Description, e => e.Language, e => e.StartsAt, e => e.EndsAt))
            {
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }

#nullable disable
        [BindProperty]
        public Event Event { get; set; }
    }
}
