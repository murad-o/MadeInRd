using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExporterWeb.Areas.Identity.Authorization;
using ExporterWeb.Helpers;
using ExporterWeb.Helpers.Services;
using ExporterWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ExporterWeb.Pages.Admin.Industries
{
    [Authorize(Roles = Constants.AdministratorsRole)]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ImageService _imageService;

        public IndexModel(ApplicationDbContext context, ImageService imageService)
        {
            _context = context;
            _imageService = imageService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            SecondaryLanguages = Languages.WhiteList;
            SecondaryLanguages.Remove(Languages.DefaultLanguage);

            IndustryTranslations =  await _context.IndustryTranslations!
                .Include(i => i.Industry)
                .ThenInclude(i => i!.Translations)
                .Where(i => i.Language == Languages.DefaultLanguage).ToListAsync();

            return Page();
        }
        
        public async Task<IActionResult> OnPostDeleteIndustryAsync(int id)
        {
            var industry = await _context.Industries!
                .Include(i => i.Translations)
                .FirstOrDefaultAsync(i => i.Id == id);
            
            if (industry is null)
            {
                return NotFound();
            }
            
            _context.Remove(industry);
            await _context.SaveChangesAsync();
            
            industry.Translations!
                .Where(t => t.Image is { })
                .ToList()
                .ForEach(t => _imageService.Delete(ImageTypes.IndustryImage, t.Image!));

            return RedirectToPage("./Index");
        }
        
        public async Task<IActionResult> OnPostDeleteTranslationAsync(int id)
        {
            var industryTranslation = await _context.IndustryTranslations!
                .Include(i => i.Industry)
                .ThenInclude(i => i!.Translations)
                .FirstOrDefaultAsync(i => i.Id == id);
            
            if (industryTranslation is null)
            {
                return NotFound();
            }

            if (industryTranslation.Language == Languages.DefaultLanguage)
            {
                return BadRequest();
            }
        
            if (industryTranslation.Industry!.Translations!.Count == 1)
            {
                _context.Industries!.Remove(industryTranslation.Industry);
            }
            else
            {
                _context.IndustryTranslations!.Remove(industryTranslation);
            }
            
            await _context.SaveChangesAsync();

            if (industryTranslation.Image is { })
            {
                _imageService.Delete(ImageTypes.IndustryImage, industryTranslation.Image);
            }
            
            return RedirectToPage("./Index");
        }
        
        #nullable disable
        public IList<IndustryTranslation> IndustryTranslations { get; set; }

        public HashSet<string> SecondaryLanguages { get; private set; }
    }
}