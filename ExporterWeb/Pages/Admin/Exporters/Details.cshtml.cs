using System;
using System.Threading.Tasks;
using ExporterWeb.Helpers;
using ExporterWeb.Helpers.Services;
using ExporterWeb.Models;
using ExporterWeb.Models.ViewModels;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ExporterWeb.Pages.Admin.Exporters
{
    public class Details : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;
        private readonly RazorPartialToStringRenderer _partialToStringRenderer;

        public Details(ApplicationDbContext context, IEmailSender emailSender, RazorPartialToStringRenderer partialToStringRenderer)
        {
            _context = context;
            _emailSender = emailSender;
            _partialToStringRenderer = partialToStringRenderer;
        }

        public async Task<IActionResult> OnGetAsync(string? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            LanguageExporter = await _context.LanguageExporters!
                .Include(e => e.CommonExporter)
                .FirstOrDefaultAsync(e => e.CommonExporterId == id && e.Language == Languages.DefaultLanguage);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            LanguageExporter = await _context.LanguageExporters!
                .Include(e => e.CommonExporter)
                .Include(e => e.CommonExporter!.User)
                .FirstOrDefaultAsync(e => e.CommonExporterId == id && e.Language == Languages.DefaultLanguage);

            if (LanguageExporter is null)
            {
                return NotFound();
            }

            LanguageExporter!.CommonExporter!.Status = Status.ToString();
            await _context.SaveChangesAsync();
            await SendStatusChangedMessage(LanguageExporter);   

            return RedirectToPage("./Index");
        }

        private async Task SendStatusChangedMessage(LanguageExporter exporter)
        {
            if (Status == ExporterStatus.Approved)
            {
                await SendApprovedStatusMessage(LanguageExporter);
            }
            else if (Status == ExporterStatus.Refused) 
            {
                await SendRefusedStatusMessage(LanguageExporter);
            }
        }

        private async Task SendApprovedStatusMessage(LanguageExporter exporter)
        {
            var email = exporter.CommonExporter!.User!.Email;
            var body =  await _partialToStringRenderer.RenderPartialToStringAsync(
                "Emails/AccountApprovedNotificationEmail",
                new ContactInfoModel
                    { FirstName = exporter.ContactPersonFirstName, LastName = exporter.ContactPersonSecondName });

            await _emailSender.SendEmailAsync(email, "Аккаунт одобрен", body);
        }

        private async Task SendRefusedStatusMessage(LanguageExporter exporter)
        {
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
        }
        

#nullable disable
        public LanguageExporter LanguageExporter { get; set; }
        
        [BindProperty]
        public ExporterStatus Status { get; set; }

        [BindProperty]
        public string RejectionReason { get; set; }
    }
}