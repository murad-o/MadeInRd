using ExporterWeb.Models;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace ExporterWeb.Pages.Events
{
    public class IndexModel : BasePageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task OnGetAsync(int pageNumber = 1)
        {
            Events = await _context.Events
                .Where(e => e.Language == CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                .ToPagedListAsync(pageNumber, pageSize: 6);
        }

#nullable disable
        public IPagedList<Event> Events { get; set; }
    }
}
