using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExporterWeb.Areas.Identity.Authorization;
using ExporterWeb.Helpers;
using ExporterWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace ExporterWeb.Pages.Admin.Exporters
{
    public class Index : PageModel
    {
        private readonly ApplicationDbContext _context;
        private const int PageSize = 4;

        public Index(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int p = 1)
        {
            if (Enum.GetNames(typeof(ExporterStatus)).All(x => x.ToLower() != Status))
            {
                return NotFound();
            }

            var exporters = _context.LanguageExporters!
                .Include(e => e.CommonExporter)
                .Where(exporter => exporter.Language == Languages.DefaultLanguage &&
                                   exporter.CommonExporter!.Status.ToLower() == Status);

            if (!string.IsNullOrEmpty(SearchString))
            {
                exporters = exporters.Where(e =>
                    e.Name.ToUpper().Contains(SearchString.ToUpper()) ||
                    e.CommonExporter!.INN.Contains(SearchString) ||
                    e.CommonExporter.OGRN_IP.Contains(SearchString) ||
                    e.ContactPersonSecondName.ToUpper().Contains(SearchString.ToUpper()) ||
                    e.ContactPersonFirstName.ToUpper().Contains(SearchString.ToUpper()) ||
                    e.ContactPersonPatronymic!.ToUpper().Contains(SearchString.ToUpper()) ||
                    e.DirectorSecondName!.ToUpper().Contains(SearchString.ToUpper()) ||
                    e.DirectorFirstName!.ToUpper().Contains(SearchString.ToUpper()) ||
                    e.DirectorPatronymic!.ToUpper().Contains(SearchString.ToUpper()) ||
                    e.CommonExporter.Industry!.Translations!
                            .FirstOrDefault(t => t.Language == Languages.DefaultLanguage)!
                        .Name.ToUpper().Contains(SearchString.ToUpper()));
            }

            Exporters = await exporters.Skip((p - 1) * PageSize).Take(PageSize).ToListAsync();

            PagingInfo = new PagingInfo
            {
                PageNumber = p,
                PageSize = PageSize,
                TotalItems = Exporters.Count
            };

            return Page();
        }

#nullable disable
        public List<LanguageExporter> Exporters { get; set; }
        public PagingInfo PagingInfo { get; set; }
        
        [BindProperty(Name = "status", SupportsGet = true)]
        public string Status { get; set; } = ExporterStatus.OnModeration.ToString();
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }
    }
}