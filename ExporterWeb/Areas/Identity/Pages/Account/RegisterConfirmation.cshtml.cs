using System.Text;
using System.Threading.Tasks;
using ExporterWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using ExporterWeb.Helpers.Services;
using ExporterWeb.Models.ViewModels;

namespace ExporterWeb.Areas.Identity.Pages.Account
{
    public class RegisterConfirmationModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly RazorPartialToStringRenderer _partialToStringRenderer;

        public RegisterConfirmationModel(UserManager<User> userManager, IEmailSender emailSender, RazorPartialToStringRenderer partialToStringRenderer)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _partialToStringRenderer = partialToStringRenderer;
        }

        public string Email { get; set; } = "";

        public string EmailConfirmationUrl { get; set; } = "";

        public async Task<IActionResult> OnGetAsync(string email, string returnUrl)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound($"Unable to load user with email '{email}'.");
            }

            Email = email;
            
            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId = userId, code, returnUrl },
                protocol: Request.Scheme);

            var body = await _partialToStringRenderer.RenderPartialToStringAsync("Emails/RegisterConfirmationEmail",
                new RegisterConfirmationEmailModel { FirstName = user.FirstName, LastName = user.SecondName, Callback = callbackUrl});

            await _emailSender.SendEmailAsync(email, "Confirm your email", body);

            return Page();
        }
    }
}
