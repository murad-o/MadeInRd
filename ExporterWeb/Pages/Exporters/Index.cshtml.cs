using ExporterWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace ExporterWeb.Pages.Exporters
{
    public class IndexModel : BasePageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            UserManager = userManager;
        }


        public async Task OnGetAsync(int? industry, string search, int pageNumber = 1)
        {
            IQueryable<LanguageExporter> exporters = _context.LanguageExporters
                .Where(e => e.Language == CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                .Include(l => l.CommonExporter)
                .Include(l => l.CommonExporter!.FieldOfActivity)
                .Include(l => l.CommonExporter!.User);

            if (!IsAdminOrManager)
                exporters = exporters.Where(e => e.Approved || e.CommonExporterId == UserId);

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower();
                exporters = exporters
                    .Where(e =>
                        e.Name.ToLower().Contains(search) ||
                        e.Description != null && e.Description.ToLower().Contains(search) ||
                        e.CommonExporter!.INN.Contains(search));
            }

            if (industry is { })
                exporters = exporters.Where(exporter => exporter.CommonExporter!.FieldOfActivity!.Id == industry);

            LanguageExporters = await exporters.ToPagedListAsync(pageNumber, pageSize: 16);
            /* temporary */
            FakeExporters = Enumerable.Repeat(exporters, 40).SelectMany(x => x).ToPagedList(pageNumber, 16);


            Industries = await _context.FieldsOfActivity.ToListAsync();

        }

    public bool CanCRUD(LanguageExporter exporter) =>
            IsAdminOrManager || exporter.CommonExporterId == UserId;

#nullable disable
        public IPagedList<LanguageExporter> LanguageExporters { get; set; }
        public IPagedList<LanguageExporter> FakeExporters { get; set; }
        public IEnumerable<FieldOfActivity> Industries { get; set; }
    }
}
