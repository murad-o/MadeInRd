using ExporterWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using ExporterWeb.Helpers.Services;

namespace ExporterWeb.Areas.Identity.Pages.Account
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly ErrorsLocalizationService _errorsLocalizer;

        public ForgotPasswordModel(UserManager<User> userManager, IEmailSender emailSender, ErrorsLocalizationService errorsLocalizer)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _errorsLocalizer = errorsLocalizer;
        }

        public class InputModel
        {
            [Required(ErrorMessage = "This field is required")]
            [EmailAddress(ErrorMessage = "Invalid email address")]
            [Display(Name = "Email", ResourceType = typeof(Resources.Account.Account))]
            public string Email { get; set; } = "";
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) 
                return Page();
            
            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null || !await _userManager.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError(string.Empty, _errorsLocalizer["This e-mail is not registered or not confirmed"]);
                return Page();
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ResetPassword",
                pageHandler: null,
                values: new { area = "Identity", code, email = Input.Email },
                protocol: Request.Scheme);

            await _emailSender.SendEmailAsync(
                Input.Email,
                "Reset Password",
                $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            return RedirectToPage("./ForgotPasswordConfirmation");

        }

#nullable disable

        [BindProperty]
        public InputModel Input { get; set; }
    }
}
