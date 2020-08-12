using ExporterWeb.Areas.Identity.Authorization;
using ExporterWeb.Helpers;
using ExporterWeb.Helpers.Services;
using ExporterWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ExporterWeb.Pages.Events
{
    [Authorize(Roles = Constants.AdministratorsRole + ", " + Constants.ManagersRole)]
    [ValidateModel]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ImageService _imageService;

        public EditModel(ApplicationDbContext context, ImageService imageService)
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

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var eventToUpdate = await _context.Events!.FindAsync(id);

            if (eventToUpdate is null) 
                return NotFound();

            var oldLogo = eventToUpdate.Logo;
            if (Logo is { })
                eventToUpdate.Logo = _imageService.Save(ImageTypes.EventLogo, Logo);

            if (!await TryUpdateModelAsync(
                eventToUpdate,
                "Event",
                e => e.Name, e => e.Description, e => e.Language, e => e.StartsAt, e => e.EndsAt))
                return RedirectToPage("./Index");
            try
            {
                await _context.SaveChangesAsync();
                if (oldLogo is { } && Logo is { })
                    _imageService.Delete(ImageTypes.EventLogo, oldLogo);
            }
            catch
            {
                if (Logo is { })
                    _imageService.Delete(ImageTypes.EventLogo, eventToUpdate.Logo!);
                throw;
            }

            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostDeleteImage(int id)
        {
            Event @event = await _context.Events!.FindAsync(id);
            _imageService.Delete(ImageTypes.EventLogo, @event.Logo!);
            @event.Logo = null;
            await _context.SaveChangesAsync();
            return StatusCode(StatusCodes.Status204NoContent);
        }

#nullable disable
        [BindProperty]
        public Event Event { get; set; }
        [BindProperty]
        public IFormFile Logo { get; set; }
    }
}
