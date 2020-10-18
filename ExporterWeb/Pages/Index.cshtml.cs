using ExporterWeb.Helpers;
using ExporterWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ExporterWeb.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ApplicationDbContext _context;

        public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IList<NewsModel>? News { get; private set; }
        public IList<FieldOfActivity>? FieldsOfActivity { get; private set; }
        public IList<Event>? Events { get; private set; }

        public async Task<IActionResult> OnGetAsync()
        {
            string path = Request.Path.Value.Split('/')[1].ToLower();
           
            if (path.Length > 0 && !Languages.WhiteList.Contains(path))
                return NotFound();

            News = await _context.News
                .Where(n => n.Language == CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                .OrderByDescending(n => n.CreatedAt)
                .Take(8)
                .ToListAsync();

            FieldsOfActivity = await _context.FieldsOfActivity
                .Take(8)
                .ToListAsync();

            Events = await _context.Events
                .Where(n => n.Language == CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                .OrderByDescending(n => n.CreatedAt)
                .Take(4)
                .ToListAsync();

            return Page();
        }
    }
}
