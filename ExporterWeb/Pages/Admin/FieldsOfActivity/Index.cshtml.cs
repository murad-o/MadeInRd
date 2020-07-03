using ExporterWeb.Helpers;
using ExporterWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExporterWeb.Pages.Admin.FieldsOfActivity
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public string Language { get; set; } = Languages.DefaultLanguage;

        public IList<FieldOfActivity>? FieldsOfActivity { get; set; }

        public async Task OnGetAsync()
        {
            FieldsOfActivity = await _context.FieldsOfActivity.ToListAsync();
        }
    }
}
