using ExporterWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace ExporterWeb.Pages.Admin.Users
{
    public class DeleteModel : PageModel
    {
        private readonly UserManager<User> _userManager;

        public DeleteModel(UserManager<User> userManager)
        {
            _userManager = userManager;
        }


        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            User = await _userManager.FindByIdAsync(id);

            if (User is null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            User = await _userManager.FindByIdAsync(id);

            if (User is { })
            {
                IdentityResult result = await _userManager.DeleteAsync(User);
                if (result.Succeeded)
                    return RedirectToPage("./Index");
            }
            else
                ModelState.AddModelError("", "User Not Found");

            return Page();
        }

#nullable disable
        [BindProperty]
        public new User User { get; set; }
    }
}
