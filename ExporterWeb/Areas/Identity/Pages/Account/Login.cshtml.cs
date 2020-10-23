using ExporterWeb.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using ExporterWeb.Helpers.Services;

namespace ExporterWeb.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ErrorsLocalizationService _errorsLocalizer;

        public LoginModel(SignInManager<User> signInManager, 
            UserManager<User> userManager, ErrorsLocalizationService errorsLocalizer)
        {
            _userManager = userManager;
            _errorsLocalizer = errorsLocalizer;
            _signInManager = signInManager;
        }

        public async Task OnGetAsync(string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (!ModelState.IsValid)
                return Page();
            
            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user is null)
            {
                ModelState.AddModelError(string.Empty, _errorsLocalizer["This e-mail is not registered"]);
                return Page();
            }
            
            var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, isPersistent: true, lockoutOnFailure: false);
            
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, _errorsLocalizer["Invalid password"]);
                return Page();
            }

            return LocalRedirect(returnUrl);
        }

#nullable disable
        public class InputModel
        {
            [Required(ErrorMessage = "This field is required")]
            [EmailAddress(ErrorMessage = "Invalid email address")]
            [Display(Name = "Email", ResourceType = typeof(Resources.Account.Account))]
            public string Email { get; set; }

            [Required(ErrorMessage = "This field is required")]
            [DataType(DataType.Password)]
            [Display(Name = "Password", ResourceType = typeof(Resources.Account.Account))]
            public string Password { get; set; }
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }
    }
}
