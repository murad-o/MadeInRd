using System.Threading.Tasks;
using ExporterWeb.Helpers;
using ExporterWeb.Helpers.Services;
using ExporterWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ExporterWeb.Pages.Admin.Industries
{
    public class Create : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ImageService _imageService;

        public Create(ApplicationDbContext context, ImageService imageService)
        {
            _context = context;
            _imageService = imageService;
        }

        public void OnGet()
        {
            
        }
        
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Industry industry = new Industry();
            var maxOrder = (await _context.Industries!.MaxAsync(x => (int?)x.Order)) ?? 0;
            industry.Order = maxOrder + 1;
            
            if (Image is { })
            {
                IndustryTranslation.Image = _imageService.Save(ImageTypes.IndustryImage, Image);
            }

            IndustryTranslation.Language = Languages.DefaultLanguage;
            industry.Translations!.Add(IndustryTranslation);
            await _context.Industries!.AddAsync(industry);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                if (Image is { })
                    _imageService.Delete(ImageTypes.NewsLogo, IndustryTranslation.Image!);
                throw;
            }
            return RedirectToPage("./Index");
        }

#nullable disable
        [BindProperty]
        public IndustryTranslation IndustryTranslation { get; set; }
        [BindProperty]
        public IFormFile Image { get; set; }
    }
}