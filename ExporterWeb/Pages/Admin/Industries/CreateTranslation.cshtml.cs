using System.Threading.Tasks;
using ExporterWeb.Helpers;
using ExporterWeb.Helpers.Services;
using ExporterWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ExporterWeb.Pages.Admin.Industries
{
    public class CreateTranslation : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ImageService _imageService;

        public CreateTranslation(ApplicationDbContext context, ImageService imageService)
        {
            _context = context;
            _imageService = imageService;
        }

        public IActionResult OnGet(int? industryId, string? lang)
        {
            if (industryId is null || lang is null)
            {
                return NotFound();
            }
            
            IndustryId = industryId.Value;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            Translation.IndustryId = IndustryId;

            if (Image is { })
            {
                Translation.Image = _imageService.Save(ImageTypes.IndustryImage, Image);
            }

            await _context.IndustryTranslations!.AddAsync(Translation);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                if (Image is { })
                {
                    _imageService.Delete(ImageTypes.IndustryImage, Translation.Image!);
                }
                throw;
            }
            return RedirectToPage("./Index");
        }
        
        #nullable disable
        [BindProperty]
        public IndustryTranslation Translation { get; set; }

        [BindProperty]
        public int IndustryId { get; set; }

        [BindProperty(Name = "lang", SupportsGet = true)]
        public string Language { get; set; }
        
        [BindProperty]
        public IFormFile Image { get; set; }
    }
}