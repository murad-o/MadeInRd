using ExporterWeb.Areas.Identity.Authorization;
using ExporterWeb.Helpers;
using ExporterWeb.Helpers.Services;
using ExporterWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ExporterWeb.Pages.Admin.News
{
    [Authorize(Roles = Constants.AdministratorsRole + ", " + Constants.ManagersRole)]
    public class EditModel : BasePageModel
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

            NewsItem = await _context.News.FirstOrDefaultAsync(m => m.Id == id);

            if (NewsItem is null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id, IFormFile? logo)
        {
            if (!ModelState.IsValid)
                return Page();

            var newsItemToUpdate = await _context.News!.FindAsync(id);
            if (newsItemToUpdate is null)
                return NotFound();

            var oldLogo = newsItemToUpdate.Logo;
            if (logo is { })
                newsItemToUpdate.Logo = _imageService.Save(ImageType.NewsLogo, logo);

            if (await TryUpdateModelAsync(
                    newsItemToUpdate,
                    "NewsItem",
                    n => n.Name, n => n.Description, n => n.Language))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    if (oldLogo is { })
                        _imageService.Delete(ImageType.NewsLogo, oldLogo);
                }
                catch
                {
                    if (logo is { })
                        _imageService.Delete(ImageType.NewsLogo, newsItemToUpdate.Logo!);
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostDeleteImage(int id)
        {
            NewsModel newsItem = await _context.News!.FindAsync(id);
            _imageService.Delete(ImageType.NewsLogo, newsItem.Logo!);
            newsItem.Logo = null;
            await _context.SaveChangesAsync();
            return StatusCode(StatusCodes.Status204NoContent);
        }

#nullable disable
        [BindProperty]
        public NewsModel NewsItem { get; set; }
    }
}
