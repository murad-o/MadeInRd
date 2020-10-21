using System.Collections.Generic;
using System.Threading.Tasks;
using ExporterWeb.Helpers.Services;
using ExporterWeb.Models.ViewModels;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ExporterWeb.Pages
{
    public class SendEmail : PageModel
    {
        private readonly IEmailSender _emailSender;
        private readonly RazorPartialToStringRenderer _razorPartialToStringRenderer;
        
        public SendEmail(IEmailSender emailSender, RazorPartialToStringRenderer razorPartialToStringRenderer)
        {
            _emailSender = emailSender;
            _razorPartialToStringRenderer = razorPartialToStringRenderer;
        }

        public async Task OnGet()
        {
            var emailModel = new ForgotPasswordEmailModel
            {
                FirstName = "Ибрагим",
                LastName = "Куданов",
                Callback = "/"
            };

            var emails = new List<string>
            {
                "ikudanov@gmail.com",
                "kudanoff@yandex.ru",
                "ibragim.kudanov@mail.ru"
            };
            var body = await _razorPartialToStringRenderer.RenderPartialToStringAsync("Emails/ForgotPasswordConfirmationEmail",
                emailModel);
            foreach (var email in emails)
            {
                await _emailSender.SendEmailAsync(email, "Test email", body);
            }
            
            
        }
    }
}