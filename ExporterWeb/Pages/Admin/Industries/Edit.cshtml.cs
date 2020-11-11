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
    [ValidateModel]
    public class Edit : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ImageService _imageService;

        public Edit(ApplicationDbContext context, ImageService imageService)
        {
            _context = context;
            _imageService = imageService;
        }

        public async Task<IActionResult> OnGet(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            Industry = await _context.IndustryTranslations!.FirstOrDefaultAsync(i => i.Id == id);

            if (Industry is null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var industryToUpdate = await _context.IndustryTranslations!.FirstOrDefaultAsync(i => i.Id == id);

            if (industryToUpdate is null || !Languages.WhiteList.Contains(Industry.Language))
            {
                return NotFound();
            } 
            
            var oldImage = industryToUpdate.Image;
            if (Image is { })
            {
                industryToUpdate.Image = _imageService.Save(ImageTypes.IndustryImage, Image);
            }

            if (!await TryUpdateModelAsync(
                industryToUpdate,
                "Industry",
                i => i.Name, i => i.Description, i => i.Language))
            {
                return RedirectToPage("./Index");
            }
            
            try
            {
                await _context.SaveChangesAsync();
                if (oldImage is { } && Image is { })
                {
                    _imageService.Delete(ImageTypes.IndustryImage, oldImage);
                }
            }
            catch
            {
                if (Image is { })
                {
                    _imageService.Delete(ImageTypes.IndustryImage, Industry.Image!);
                }
                throw;
            }
            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostDeleteImage(int id)
        {
            Industry = await _context.IndustryTranslations!.FindAsync(id);
            _imageService.Delete(ImageTypes.IndustryImage, Industry.Image!);
            Industry.Image = null;
            await _context.SaveChangesAsync();
            return StatusCode(StatusCodes.Status204NoContent);
        }

#nullable disable
        [BindProperty]
        public IndustryTranslation Industry { get; set; }
        [BindProperty]
        public IFormFile Image { get; set; }
    }
}