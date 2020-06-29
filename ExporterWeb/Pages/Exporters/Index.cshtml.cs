using ExporterWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExporterWeb.Pages.Exporters
{
    [AllowAnonymous]
    public class IndexModel : BasePageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public IndexModel(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<LanguageExporter> LanguageExporters { get; set; } = Array.Empty<LanguageExporter>();

        [BindProperty(Name = "my", SupportsGet = true)]
        public bool ShowOnlyMyExporters { get; set; }
        [BindProperty(Name = "q", SupportsGet = true)]
        public string? Search { get; set; }

        public async Task OnGetAsync()
        {
            Init(_userManager);
            IQueryable<LanguageExporter> exporters = _context.LanguageExporters
                .Include(l => l.CommonExporter)
                .Include(l => l.CommonExporter!.FieldOfActivity)
                .Include(l => l.CommonExporter!.User);

            exporters = ShowOnlyMyExporters
                ? exporters.Where(e => e.CommonExporterId == UserId)
                : exporters.Where(e => e.Language == Language);
            if (!IsAdminOrManager)
            {
                exporters = exporters
                    .Where(e => e.Approved || e.CommonExporterId == UserId);
            }
            if (!string.IsNullOrWhiteSpace(Search))
            {
                string normalizedSearch = Search.ToLower();
                exporters = exporters
                    .Where(e =>
                        e.Name.ToLower().Contains(normalizedSearch) ||
                        e.Description != null && e.Description.ToLower().Contains(normalizedSearch));
            }
                
            LanguageExporters = await exporters.ToListAsync();
        }
    }
}
