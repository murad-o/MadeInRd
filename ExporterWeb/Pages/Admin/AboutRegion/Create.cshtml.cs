using ExporterWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using System.Threading.Tasks;
using ExporterWeb.Helpers.Services;

namespace ExporterWeb.Pages.Admin.AboutRegion
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ImageResizeService _imageResizer;

        public CreateModel(ApplicationDbContext context, ImageResizeService imageResizer)
        {
            _context = context;
            _imageResizer = imageResizer;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (_context.AboutRegionContents!.Any(x => x.Lang == AboutRegionModel.Lang))
                return BadRequest();

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.AboutRegionContents!.Add(AboutRegionModel);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

#nullable disable
        [BindProperty]
        public AboutRegionModel AboutRegionModel { get; set; }
    }
}
