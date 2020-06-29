using ExporterWeb.Areas.Identity.Authorization;
using ExporterWeb.Helpers;
using ExporterWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ExporterWeb.Pages.Exporters
{
    public class CreateModel : BasePageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthorizationService _authorizationService;
        private readonly UserManager<User> _userManager;

        public CreateModel(ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<User> userManager)
        {
            _context = context;
            _authorizationService = authorizationService;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Init(_userManager);
            // TODO: authorize and add Where(exporters => exporters.UserId == current_user_id)
            var exporters = _context.CommonExporters.Include(nameof(CommonExporter.User));
            if (!IsAdminOrManager)
                exporters = exporters.Where(e => e.UserId == UserId);

            var items = await exporters.ToListAsync();
            ViewData["CommonExporterList"] = new SelectList(items, "UserId", "User.Email");
            ViewData["CommonExporterFirstId"] = items.FirstOrDefault()?.UserId;
            ViewData["WhiteListLanguages"] = new SelectList(Languages.WhiteList);
            return Page();
        }

        private async Task<bool> IsAuthorized()
        {
            var result = await _authorizationService.AuthorizeAsync(
                User, LanguageExporter, AuthorizationOperations.Create);
            return result.Succeeded;
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || !Languages.WhiteList.Contains(LanguageExporter.Language))
                return Page();

            if (await IsAuthorized())
            {
                // If the user is a regular person, mark it as pending
                if (!User.IsInRole(Constants.AdministratorsRole) &&
                    !User.IsInRole(Constants.ManagersRole))
                {
                    LanguageExporter.Approved = false;
                }
                _context.LanguageExporters!.Add(LanguageExporter);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("./Details", new { id = LanguageExporter.CommonExporterId, LanguageExporter.Language });
        }

#nullable disable
        [BindProperty]
        public LanguageExporter LanguageExporter { get; set; }
    }
}
