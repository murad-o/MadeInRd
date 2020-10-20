using ExporterWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Threading.Tasks;
using ExporterWeb.Helpers.Services;
using ExporterWeb.Models.ViewModels;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace ExporterWeb.Areas.Identity.Pages.Account
{
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailSender _emailSender;
        private RazorPartialToStringRenderer _razorPartialToStringRenderer;

        public ConfirmEmailModel(UserManager<User> userManager, IEmailSender emailSender, RazorPartialToStringRenderer razorPartialToStringRenderer)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _razorPartialToStringRenderer = razorPartialToStringRenderer;
        }

        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            var body = await _razorPartialToStringRenderer.RenderPartialToStringAsync(
                "Emails/RegistrationCompletedEmail", new RegisterConfirmationEmailModel());
            await _emailSender.SendEmailAsync(user.Email, "Email confirmed", body);
            return Page();
        }
    }
}
