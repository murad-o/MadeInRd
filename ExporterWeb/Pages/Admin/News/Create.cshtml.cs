using ExporterWeb.Areas.Identity.Authorization;
using ExporterWeb.Helpers;
using ExporterWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Threading.Tasks;

namespace ExporterWeb.Pages.Admin.News
{
    [Authorize(Roles = Constants.AdministratorsRole + ", " + Constants.ManagersRole)]
    public class CreateModel : BasePageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public CreateModel(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult OnGet()
        {
            Init(_userManager);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["WhiteListLanguages"] = new SelectList(Languages.WhiteList);
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            Init(_userManager);
            // To prevent user falsify "UserNameOwner" and "CreatedAt"
            NewsItem.UserNameOwner = User.Identity.Name!;
            NewsItem.CreatedAt = DateTime.Now;

            _context.News!.Add(NewsItem);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

#nullable disable
        [BindProperty]
        public NewsModel NewsItem { get; set; }
    }
}
