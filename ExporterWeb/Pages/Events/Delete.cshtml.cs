using ExporterWeb.Areas.Identity.Authorization;
using ExporterWeb.Helpers;
using ExporterWeb.Helpers.Services;
using ExporterWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ExporterWeb.Pages.Events
{
    [Authorize(Roles = Constants.AdministratorsRole + ", " + Constants.ManagersRole)]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ImageService _imageService;

        public DeleteModel(ApplicationDbContext context, ImageService imageService)
        {
            _context = context;
            _imageService = imageService;
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id is null)
                return NotFound();

            Event = await _context.Events!.FindAsync(id);

            if (Event is { })
            {
                _context.Events.Remove(Event);
                await _context.SaveChangesAsync();
                if (Event.Logo is { })
                    _imageService.Delete(ImageTypes.EventLogo, Event.Logo);
            }

            return RedirectToPage("./Index");
        }

#nullable disable
        [BindProperty]
        public Event Event { get; set; }
    }
}
