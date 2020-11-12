using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ExporterWeb.Pages.Admin.Exporters
{
    public class Edit : PageModel
    {
        public IActionResult OnGet()
        {
            return Page();
        }
    }
}