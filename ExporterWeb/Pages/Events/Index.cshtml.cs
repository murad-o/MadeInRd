using ExporterWeb.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExporterWeb.Pages.Events
{
    public class IndexModel : BasePageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task OnGetAsync()
        {
            Events = await _context.Events.ToListAsync();
        }

#nullable disable
        public IList<Event> Events { get;set; }
    }
}
