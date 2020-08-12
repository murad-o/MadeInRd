using ExporterWeb.Areas.Identity.Authorization;
using ExporterWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace ExporterWeb.Pages.Admin.Users
{
    [BindProperties]
    public class EditModel : PageModel
    {
        private readonly UserManager<User> _userManager;

        public EditModel(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id is null)
                return NotFound();

            User = await _userManager.FindByIdAsync(id);
            IsManager = await _userManager.IsInRoleAsync(User, Constants.ManagersRole);
            IsAnalyst = await _userManager.IsInRoleAsync(User, Constants.AnalystsRole);

            if (User is null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            User user = await _userManager.FindByIdAsync(User.Id);
            if (user is null)
                return Page();
            
            user.UserName = User.UserName;
            user.FirstName = User.FirstName;
            user.SecondName = User.SecondName;
            user.Patronymic = User.Patronymic;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                var userIsManager = await _userManager.IsInRoleAsync(user, Constants.ManagersRole);
                var userIsAnalyst = await _userManager.IsInRoleAsync(user, Constants.AnalystsRole);
                if (IsManager)
                {
                    if (!userIsManager)
                        await _userManager.AddToRoleAsync(user, Constants.ManagersRole);
                }
                else
                {
                    if (userIsManager)
                        await _userManager.RemoveFromRoleAsync(user, Constants.ManagersRole);
                }

                if (IsAnalyst)
                {
                    if (!userIsAnalyst)
                        await _userManager.AddToRoleAsync(user, Constants.AnalystsRole);
                }
                else
                {
                    if (userIsManager)
                        await _userManager.RemoveFromRoleAsync(user, Constants.AnalystsRole);
                }

                return RedirectToPage("./Index");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
            
            return Page();
        }

#nullable disable
        public new User User { get; set; }
        public bool IsAnalyst { get; set; }
        public bool IsManager { get; set; }
    }
}
