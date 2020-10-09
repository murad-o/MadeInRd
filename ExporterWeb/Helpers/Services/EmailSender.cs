using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ExporterWeb.Helpers.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var smtpSection = _configuration.GetSection("smtp");
            var smtpEmail = smtpSection["email"];

            var from = new MailAddress(smtpEmail, "Made in RD", System.Text.Encoding.UTF8);
            var to = new MailAddress(email);

            using var mailMessage = new MailMessage(from, to)
            {
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };

            using var client = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                EnableSsl = true,
                Credentials = new NetworkCredential(smtpEmail, smtpSection["password"])
            };

            await client.SendMailAsync(mailMessage);
        }
    }
}
