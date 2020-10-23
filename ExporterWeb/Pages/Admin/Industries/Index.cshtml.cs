using ExporterWeb.Areas.Identity.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ExporterWeb.Pages.Admin.Industries
{
    [Authorize(Roles = Constants.AdministratorsRole)]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
            
        }
    }
}