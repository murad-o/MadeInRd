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
using Microsoft.AspNetCore.Identity.UI.Services;

namespace ExporterWeb.Areas.Identity.Pages.Account
{
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly RazorPartialToStringRenderer _razorPartialToStringRenderer;
        private readonly IEmailSender _emailSender;

        public ResetPasswordModel(UserManager<User> userManager, RazorPartialToStringRenderer razorPartialToStringRenderer, IEmailSender emailSender)
        {
            _userManager = userManager;
            _razorPartialToStringRenderer = razorPartialToStringRenderer;
            _emailSender = emailSender;
        }

        public IActionResult OnGet(string code, string email)
        {
            Input = new ResetPasswordViewModel
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
                    "Emails/ResetPasswordConfirmationEmail",
                    new ContactInfoModel {FirstName = user.FirstName, LastName = user.SecondName});
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
        public ResetPasswordViewModel Input { get; set; }
    }
}
