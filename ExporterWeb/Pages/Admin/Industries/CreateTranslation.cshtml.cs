using System.Collections.Generic;
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

        public IActionResult OnGet(int? industryId)
        {
            if (industryId is null)
            {
                return NotFound();
            }
            
            IndustryId = industryId.Value;
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
    }
}