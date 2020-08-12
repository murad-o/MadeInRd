using ExporterWeb.Areas.Identity.Authorization;
using ExporterWeb.Helpers;
using ExporterWeb.Helpers.Services;
using ExporterWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ExporterWeb.Pages.News
{
    [Authorize(Roles = Constants.AdministratorsRole + ", " + Constants.ManagersRole)]
    public class DeleteModel : BasePageModel
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
            if (id == null)
                return NotFound();

            NewsItem = await _context.News.FirstOrDefaultAsync(m => m.Id == id);

            if (NewsItem is null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id is null)
                return NotFound();

            NewsItem = await _context.News!.FindAsync(id);

            if (NewsItem is null) 
                return RedirectToPage("./Index");
            
            _context.News.Remove(NewsItem);
            await _context.SaveChangesAsync();
            if (NewsItem.Logo is { })
                _imageService.Delete(ImageTypes.NewsLogo, NewsItem.Logo);

            return RedirectToPage("./Index");
        }

#nullable disable
        [BindProperty]
        public NewsModel NewsItem { get; set; }
    }
}
