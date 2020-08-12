using ExporterWeb.Areas.Identity.Authorization;
using ExporterWeb.Helpers;
using ExporterWeb.Helpers.Services;
using ExporterWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace ExporterWeb.Pages.News
{
    [Authorize(Roles = Constants.AdministratorsRole + ", " + Constants.ManagersRole)]
    [ValidateModel]
    public class CreateModel : BasePageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ImageService _imageService;

        public CreateModel(ApplicationDbContext context, IWebHostEnvironment appEnvironment, ImageService imageService)
        {
            _context = context;
            _imageService = imageService;
        }

        public IActionResult OnGet()
        {
            ViewData["WhiteListLanguages"] = new SelectList(Languages.WhiteList);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var newsItem = new NewsModel { UserNameOwner = User.Identity.Name! };

            if (Logo is { })
                newsItem.Logo = _imageService.Save(ImageTypes.NewsLogo, Logo);

            if (!await TryUpdateModelAsync(
                newsItem,
                nameof(NewsItem),
                n => n.Name, n => n.Description, n => n.Language))
                return Page();
            
            await _context.News!.AddAsync(newsItem);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                if (Logo is { })
                    _imageService.Delete(ImageTypes.NewsLogo, newsItem.Logo!);
                throw;
            }
            return RedirectToPage("./Index");
        }

#nullable disable
        [BindProperty]
        public NewsModel NewsItem { get; set; }
        [BindProperty]
        public IFormFile Logo { get; set; }
    }
}
