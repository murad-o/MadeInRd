using ExporterWeb.Areas.Identity.Authorization;
using ExporterWeb.Helpers;
using ExporterWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace ExporterWeb.Pages.Admin.News
{
    [Authorize(Roles = Constants.AdministratorsRole + ", " + Constants.ManagersRole)]
    public class CreateModel : BasePageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            ViewData["WhiteListLanguages"] = new SelectList(Languages.WhiteList);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var newsItem = new NewsModel { UserNameOwner = User.Identity.Name! };
            if (await TryUpdateModelAsync(
                    newsItem,
                    nameof(NewsItem),
                    n => n.Name, n => n.Description, n => n.Language))
            {
                await _context.News!.AddAsync(newsItem);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }
            return Page();
        }

#nullable disable
        [BindProperty]
        public NewsModel NewsItem { get; set; }
    }
}
