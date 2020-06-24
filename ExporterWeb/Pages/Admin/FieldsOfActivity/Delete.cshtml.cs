using ExporterWeb.Areas.Identity.Authorization;
using ExporterWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ExporterWeb.Pages.Admin.FieldsOfActivity
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthorizationService _authorizationService;

        public DeleteModel(ApplicationDbContext context, IAuthorizationService authorizationService)
        {
            _context = context;
            _authorizationService = authorizationService;
        }

        [BindProperty]
        public FieldOfActivity? FieldOfActivity { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            if (!await IsAuthorized())
                return Forbid();

            FieldOfActivity = await _context.FieldsOfActivity.FirstOrDefaultAsync(m => m.Id == id);

            if (FieldOfActivity == null)
                return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
                return NotFound();

            if (!await IsAuthorized())
                return Forbid();

            FieldOfActivity = await _context.FieldsOfActivity!.FindAsync(id);

            if (FieldOfActivity != null)
            {
                _context.FieldsOfActivity.Remove(FieldOfActivity);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }

        private async Task<bool> IsAuthorized()
        {
            var result = await _authorizationService.AuthorizeAsync(
                            User, FieldOfActivity, AuthorizationOperations.Create);
            return result.Succeeded;
        }
    }
}
