using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExporterWeb.Helpers;
using ExporterWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ExporterWeb.Pages.Admin.Industries
{
    [ValidateModel]
    public class CreateTranslation : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateTranslation(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGet(int? industryId)
        {
            if (industryId is null)
            {
                return NotFound();
            }
            
            IndustryId = industryId.Value;
            AvailableLanguages = Languages.WhiteList.Except((await _context.Industries!
                .Include(i => i.Translations)
                .FirstOrDefaultAsync(i => i.Id == IndustryId))
                .Translations!
                .Select(t => t.Language));
            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Translation.IndustryId = IndustryId;
            await _context.IndustryTranslations!.AddAsync(Translation);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Translations", new{ Id = IndustryId });
        }
        
        #nullable disable
        [BindProperty]
        public IndustryTranslation Translation { get; set; }

        [BindProperty]
        public int IndustryId { get; set; }
        public IEnumerable<string> AvailableLanguages { get; set; }
    }
}