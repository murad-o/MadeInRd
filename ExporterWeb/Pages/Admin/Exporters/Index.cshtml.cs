using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ExporterWeb.Areas.Identity.Authorization;
using ExporterWeb.Helpers;
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
    public class Index : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;
        private readonly RazorPartialToStringRenderer _partialToStringRenderer;

        public Index(ApplicationDbContext context, IEmailSender emailSender, RazorPartialToStringRenderer partialToStringRenderer)
        {
            _context = context;
            _emailSender = emailSender;
            _partialToStringRenderer = partialToStringRenderer;
            Exporters = _context.LanguageExporters!
                .Where(exporter => exporter.CommonExporter!.Status != ExporterStatus.Approved.ToString() && 
                                   exporter.Language == CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                .ToList();
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var exporter = await _context.LanguageExporters!
                .Include(e => e.CommonExporter)
                .Include(e => e.CommonExporter!.User)
                .FirstOrDefaultAsync(e => e.CommonExporterId == Id && e.Language == Language);

            exporter.CommonExporter!.Status = ExporterStatus.Approved.ToString();
            await _context.SaveChangesAsync();
            var email = exporter.CommonExporter!.User!.Email;
            var body = await _partialToStringRenderer.RenderPartialToStringAsync(
                "Emails/AccountApprovedNotificationEmail",
                new ContactInfoModel
                    {FirstName = exporter.ContactPersonFirstName, LastName = exporter.ContactPersonSecondName});

            await _emailSender.SendEmailAsync(email, "Аккаунт одобрен", body);
            return Page();
        }


#nullable disable
        public List<LanguageExporter> Exporters { get; set; }
        [BindProperty]
        public string Id { get; set; }
        [BindProperty]
        public string Language { get; set; }
        [BindProperty]
        public string RejectionReason { get; set; }
    }
}