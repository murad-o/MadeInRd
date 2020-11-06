using System.Threading.Tasks;
using ExporterWeb.Helpers;
using ExporterWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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

        public IActionResult OnGet(int? industryId, string? lang)
        {
            if (industryId is null || lang is null)
            {
                return NotFound();
            }
            
            IndustryId = industryId.Value;
            Language = lang;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Translation.IndustryId = IndustryId;
            await _context.IndustryTranslations!.AddAsync(Translation);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
        
        #nullable disable
        [BindProperty]
        public IndustryTranslation Translation { get; set; }

        [BindProperty]
        public int IndustryId { get; set; }

        [BindProperty] 
        public string Language { get; set; }
    }
}