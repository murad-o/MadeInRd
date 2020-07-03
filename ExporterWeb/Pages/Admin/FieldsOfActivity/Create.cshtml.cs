using ExporterWeb.Areas.Identity.Authorization;
using ExporterWeb.Helpers;
using ExporterWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ExporterWeb.Pages.Admin.FieldsOfActivity
{
    public class CreateModel : BasePageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthorizationService _authorizationService;

        public CreateModel(ApplicationDbContext context, IAuthorizationService authorizationService)
        {
            _context = context;
            _authorizationService = authorizationService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!await IsAuthorized())
                return Forbid();

            return Page();
        }

        [BindProperty]
        public FieldOfActivity? FieldOfActivity { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            string? nameForDefaultLanguage = null;
            foreach (var fieldOFActivityName in LocalizedNames)
            {
                FieldOfActivity!.Name[fieldOFActivityName.Language] = fieldOFActivityName.Name;
                if (fieldOFActivityName.Language == Languages.DefaultLanguage)
                    nameForDefaultLanguage = fieldOFActivityName.Name;
            }

            if (nameForDefaultLanguage is null)
            {
                ModelState.AddModelError(string.Empty, "Name for default language is required");
                return Page();
            }

            if (!ModelState.IsValid)
                return Page();

            if (!await IsAuthorized())
                return Forbid();

            _context.FieldsOfActivity!.Add(FieldOfActivity!);
            await _context.SaveChangesAsync();

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
