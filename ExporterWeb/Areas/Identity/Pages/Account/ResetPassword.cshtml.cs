using ExporterWeb.Models;
using Microsoft.AspNetCore.Identity;
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
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly RazorPartialToStringRenderer _razorPartialToStringRenderer;
        private readonly EmailSender _emailSender;

        public ResetPasswordModel(UserManager<User> userManager, RazorPartialToStringRenderer razorPartialToStringRenderer, EmailSender emailSender)
        {
            _userManager = userManager;
            _razorPartialToStringRenderer = razorPartialToStringRenderer;
            _emailSender = emailSender;
        }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; } = "";

            [Required(ErrorMessage = "This field is required")]
            [StringLength(100, ErrorMessage = "Passwords must consist at least {2} characters", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password", ResourceType = typeof(Resources.Account.Account))]
            public string Password { get; set; } = "";

            [Required(ErrorMessage = "This field is required")]
            [DataType(DataType.Password)]
            [Display(Name = "ConfirmPassword", ResourceType = typeof(Resources.Account.Account))]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; } = "";

            public string Code { get; set; } = "";
        }

        public IActionResult OnGet(string code, string email)
        {
            Input = new InputModel
            {
                Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code)),
                Email = email
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                return Page();
            }

            var result = await _userManager.ResetPasswordAsync(user, Input.Code, Input.Password);
            if (result.Succeeded)
            {
                var emailBody = await _razorPartialToStringRenderer.RenderPartialToStringAsync(
                    "Emails/ResetPasswordConfirmationEmail", new EmailViewModel());
                await _emailSender.SendEmailAsync(user.Email, "Пароль изменен", emailBody);
                return RedirectToPage("./ResetPasswordConfirmation");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return Page();
        }

#nullable disable
        [BindProperty]
        public InputModel Input { get; set; }
    }
}
