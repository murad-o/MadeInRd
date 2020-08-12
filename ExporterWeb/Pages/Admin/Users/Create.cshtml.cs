using ExporterWeb.Areas.Identity.Authorization;
using ExporterWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace ExporterWeb.Pages.Admin.Users
{
    [BindProperties]
    public class CreateModel : PageModel
    {
        private readonly UserManager<User> _userManager;

        public CreateModel(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();
            
            var user = new User(UserName);
            var result = await _userManager.CreateAsync(user, Password);

            if (result.Succeeded)
            {
                if (IsManager)
                    await _userManager.AddToRoleAsync(user, Constants.ManagersRole);
                if (IsAnalyst)
                    await _userManager.AddToRoleAsync(user, Constants.AnalystsRole);
                return RedirectToPage("./Index");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return Page();
        }

#nullable disable
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsManager { get; set; }
        public bool IsAnalyst { get; set; }
    }
}
