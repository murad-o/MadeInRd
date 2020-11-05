using System.Threading.Tasks;
using ExporterWeb.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ExporterWeb.Pages.Industries
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task OnGet(int id)
        {
            Industry = await _context.IndustryTranslations.FirstOrDefaultAsync(i => i.Id == id);
        }
        
        #nullable disable
        public IndustryTranslation Industry { get; set; }
    }
}