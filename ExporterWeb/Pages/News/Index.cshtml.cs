using ExporterWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ExporterWeb.Pages.News
{
    public class IndexModel : BasePageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public IndexModel(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task OnGetAsync()
        {
            News = await _context.News
                .Where(e => e.Language == CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
                .ToListAsync();
        }

#nullable disable
        public IList<NewsModel> News { get; set; }
    }
}
