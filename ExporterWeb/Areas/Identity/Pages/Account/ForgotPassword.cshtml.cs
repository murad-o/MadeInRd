using ExporterWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using ExporterWeb.Helpers.Services;
using ExporterWeb.Models.ViewModels;

namespace ExporterWeb.Areas.Identity.Pages.Account
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly ErrorsLocalizationService _errorsLocalizer;
        private readonly RazorPartialToStringRenderer _razorPartialToStringRenderer;

        public ForgotPasswordModel(UserManager<User> userManager, IEmailSender emailSender, ErrorsLocalizationService errorsLocalizer, RazorPartialToStringRenderer razorPartialToStringRenderer)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _errorsLocalizer = errorsLocalizer;
            _razorPartialToStringRenderer = razorPartialToStringRenderer;
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

            var body = await _razorPartialToStringRenderer.RenderPartialToStringAsync(
                "Emails/ForgotPasswordConfirmationEmail", new ForgotPasswordEmailModel {Callback = callbackUrl});
            
            await _emailSender.SendEmailAsync(Input.Email, "Reset Password",body);

            return RedirectToPage("./ForgotPasswordConfirmation");

        }

#nullable disable

        [BindProperty]
        public InputModel Input { get; set; }
    }
}
