using ExporterWeb.Areas.Identity.Authorization;
using ExporterWeb.Helpers;
using ExporterWeb.Helpers.Services;
using ExporterWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace ExporterWeb.Pages.Events
{
    [Authorize(Roles = Constants.AdministratorsRole + ", " + Constants.ManagersRole)]
    [ValidateModel]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ImageService _imageService;

        public CreateModel(ApplicationDbContext context, ImageService imageService)
        {
            _context = context;
            _imageService = imageService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var @event = new Event { UserNameOwner = User.Identity.Name! };

            if (Logo is { })
                @event.Logo = _imageService.Save(ImageTypes.EventLogo, Logo);

            if (!await TryUpdateModelAsync(
                @event,
                nameof(Event),
                e => e.Name, e => e.Description, e => e.Language, e => e.StartsAt, e => e.EndsAt))
                return Page();
            
            await _context.Events!.AddAsync(@event);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                if (Logo is { })
                    _imageService.Delete(ImageTypes.EventLogo, @event.Logo!);
                throw;
            }
            return RedirectToPage("./Index");

        }

#nullable disable
        [BindProperty]
        public Event Event { get; set; }

        [BindProperty]
        public IFormFile Logo { get; set; }
    }
}
