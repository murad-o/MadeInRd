using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
    public class Index : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;

        public Index(ApplicationDbContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
            Exporters = _context.LanguageExporters!
                .Where(exporter => !exporter.Approved && exporter.Language == CultureInfo.CurrentCulture.TwoLetterISOLanguageName).ToList();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var exporter = await _context.LanguageExporters
                .Include(e => e.CommonExporter)
                .Include(e => e.CommonExporter.User)
                .FirstOrDefaultAsync(e => e.CommonExporterId == Id && e.Language == Language);

            exporter.Approved = true;
            await _context.SaveChangesAsync();
            var email = exporter.CommonExporter!.User!.Email;
            const string message =
                "Администрация Made in RD проверила информация вашей компании и одобрила ее. Теперь вы можете полноценно пользоваться всеми возможностями сайта";
            string subject = "Компания одобрена";

            await _emailSender.SendEmailAsync(email, subject, message);
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