using System.Globalization;
using System.Threading.Tasks;
using ExporterWeb.Areas.Identity.Authorization;
using ExporterWeb.Models;
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

        public Reject(ApplicationDbContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
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
                .FirstOrDefaultAsync(e => e.CommonExporterId == Id && e.Language == CultureInfo.CurrentCulture.TwoLetterISOLanguageName);

            var email = exporter.CommonExporter!.User!.Email;
            string message = $"Ваша компания не одобрена администрацией сайта. Причина отказа: {RejectionReason}";
            string subject = "Компания отклонена";
            await _emailSender.SendEmailAsync(email, subject, message);

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