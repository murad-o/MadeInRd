using ExporterWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace ExporterWeb.Pages.Admin.Users
{
    public class DetailsModel : PageModel
    {
        private readonly UserManager<User> _userManager;

        public DetailsModel(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id is null)
                return NotFound();

            User = await _userManager.FindByIdAsync(id);

            if (User is null)
                return NotFound();

            return Page();
        }
#nullable disable
        new public User User { get; set; }
    }
}
