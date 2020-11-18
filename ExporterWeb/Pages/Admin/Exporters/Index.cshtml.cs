using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExporterWeb.Areas.Identity.Authorization;
using ExporterWeb.Helpers;
using ExporterWeb.Helpers.Services;
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
        private readonly RazorPartialToStringRenderer _partialToStringRenderer;

        public Index(ApplicationDbContext context, IEmailSender emailSender, RazorPartialToStringRenderer partialToStringRenderer)
        {
            _context = context;
            _emailSender = emailSender;
            _partialToStringRenderer = partialToStringRenderer;
        }

        public async Task<IActionResult> OnGetAsync(string? status)
        {
            if (status is null || !Enum.IsDefined(typeof(ExporterStatus), status))
            {
                return NotFound();
            }

            Exporters = await _context.LanguageExporters!
                .Include(e => e.CommonExporter)
                .Where(exporter => exporter.Language == Languages.DefaultLanguage && exporter.CommonExporter!.Status == status)
                .ToListAsync();

            return Page();
        }

#nullable disable
        public List<LanguageExporter> Exporters { get; set; }
        
        [BindProperty]
        public string Id { get; set; }
        [BindProperty]
        public string Language { get; set; }
    }
}