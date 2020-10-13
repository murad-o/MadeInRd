using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ExporterWeb.Pages
{
    public class TermsOfUse : PageModel
    {
        public IActionResult OnGet()
        {
            return Page();
        }
    }
}