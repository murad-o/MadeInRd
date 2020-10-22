using System.Globalization;
using System.Threading.Tasks;
using ExporterWeb.Areas.Identity.Authorization;
using ExporterWeb.Helpers.Services;
using ExporterWeb.Models;
using ExporterWeb.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ExporterWeb.Pages.Admin.Exporters
{
    [Authorize(Roles = Constants.AdministratorsRole)]
    public class Reject : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;
        private readonly RazorPartialToStringRenderer _partialToStringRenderer;

        public Reject(ApplicationDbContext context, IEmailSender emailSender, RazorPartialToStringRenderer partialToStringRenderer)
        {
            _context = context;
            _emailSender = emailSender;
            _partialToStringRenderer = partialToStringRenderer;
        }

        public void OnGet(string id, string language)
        {
            Id = id;
            Language = language;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var exporter = await _context.LanguageExporters
                .Include(e => e.CommonExporter)
                .Include(e => e.CommonExporter.User)
                .FirstOrDefaultAsync(e => e.CommonExporterId == Id && e.Language == Language);

            var email = exporter.CommonExporter!.User!.Email;
            var body = await _partialToStringRenderer.RenderPartialToStringAsync(
                "Emails/AccountRefusedNotificationEmail",
                new AccountRefusedViewModel
                {
                    FirstName = exporter.ContactPersonFirstName,
                    LastName = exporter.ContactPersonSecondName,
                    RejectionReason = RejectionReason
                });
            await _emailSender.SendEmailAsync(email, "Аккаунт отклонен", body);
            return RedirectToPage("./Index");
        } 
        
        [BindProperty]
        public string Id { get; set; }
        [BindProperty]
        public string RejectionReason { get; set; }
        [BindProperty]
        public string Language { get; set; }
    }
}