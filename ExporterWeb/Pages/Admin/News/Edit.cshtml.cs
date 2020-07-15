using ExporterWeb.Areas.Identity.Authorization;
using ExporterWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ExporterWeb.Pages.Admin.News
{
    [Authorize(Roles = Constants.AdministratorsRole + ", " + Constants.ManagersRole)]
    public class EditModel : BasePageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id is null)
                return NotFound();

            NewsItem = await _context.News.FirstOrDefaultAsync(m => m.Id == id);

            if (NewsItem is null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
                return Page();

            var newsItemToUpdate = await _context.News!.FindAsync(id);
            if (newsItemToUpdate is null)
                return NotFound();

            if (await TryUpdateModelAsync(
                    newsItemToUpdate,
                    "NewsItem",
                    n => n.Name, n => n.Description, n => n.Language))
            {
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }

#nullable disable
        [BindProperty]
        public NewsModel NewsItem { get; set; }
    }
}
