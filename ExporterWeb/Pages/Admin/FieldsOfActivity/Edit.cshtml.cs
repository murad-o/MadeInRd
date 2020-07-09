using ExporterWeb.Areas.Identity.Authorization;
using ExporterWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ExporterWeb.Pages.Admin.FieldsOfActivity
{
    public class EditModel : BasePageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthorizationService _authorizationService;

        public EditModel(ApplicationDbContext context, IAuthorizationService authorizationService)
        {
            _context = context;
            _authorizationService = authorizationService;
        }

        [BindProperty]
        public FieldOfActivity? FieldOfActivity { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id is null)
                return NotFound();

            FieldOfActivity = await _context.FieldsOfActivity!.FirstOrDefaultAsync(f => f.Id == id);

            if (FieldOfActivity is null)
                return NotFound();

            if (!await IsAuthorized())
                return Forbid();

            FillFieldOfActivityNames(FieldOfActivity);
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            if (!await IsAuthorized())
                return Forbid();

            foreach (var fieldOFActivityName in LocalizedNames)
            {
                FieldOfActivity!.Name[fieldOFActivityName.Language] = fieldOFActivityName.Name;
            }

            _context.Attach(FieldOfActivity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FieldOfActivityExists(FieldOfActivity!.Id))
                    return NotFound();
                else
                    throw;
            }

            return RedirectToPage("./Index");
        }

        private bool FieldOfActivityExists(int id)
        {
            return _context.FieldsOfActivity.Any(e => e.Id == id);
        }

        private async Task<bool> IsAuthorized()
        {
            var result = await _authorizationService.AuthorizeAsync(
                            User, FieldOfActivity, AuthorizationOperations.Update);
            return result.Succeeded;
        }
    }
}
