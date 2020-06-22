using ExporterWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace ExporterWeb.Pages.Admin.FieldsOfActivity
{
    public class CreateModel : PageModel
    {
        private readonly ExporterWeb.Models.ApplicationDbContext _context;

        public CreateModel(ExporterWeb.Models.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public FieldOfActivity? FieldOfActivity { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.FieldsOfActivity!.Add(FieldOfActivity!);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
