using ExporterWeb.Models;
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

        public IList<FieldOfActivity>? FieldOfActivity { get; set; }

        public async Task OnGetAsync()
        {
            FieldOfActivity = await _context.FieldsOfActivity.ToListAsync();
        }
    }
}
