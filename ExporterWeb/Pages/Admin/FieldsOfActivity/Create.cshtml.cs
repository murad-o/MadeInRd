using ExporterWeb.Areas.Identity.Authorization;
using ExporterWeb.Helpers;
using ExporterWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ExporterWeb.Pages.Admin.FieldsOfActivity
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
            return Page();
        }

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

            _context.FieldsOfActivity!.Add(FieldOfActivity!);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

#nullable disable
        [BindProperty]
        public FieldOfActivity FieldOfActivity { get; set; }
    }
}
