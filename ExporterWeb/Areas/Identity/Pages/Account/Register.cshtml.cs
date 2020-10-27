using ExporterWeb.Areas.Identity.Authorization;
using ExporterWeb.Helpers;
using ExporterWeb.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ExporterWeb.Models.ViewModels;


namespace ExporterWeb.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly ApplicationDbContext _context;

        public RegisterModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<RegisterModel> logger,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
        }

        public string? ReturnUrl { get; set; }

        public IList<AuthenticationScheme>? ExternalLogins { get; set; }

        public IEnumerable<IndustryTranslation> Industries => _context.IndustryTranslations!
            .Where(i => i.Language == CultureInfo.CurrentCulture.TwoLetterISOLanguageName).ToList();

        public async Task OnGetAsync(string? returnUrl)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (!ModelState.IsValid)
                return Page();

            var user = new User
            {
                UserName = Input.Email,
                Email = Input.Email,
                FirstName = Input.ContactPersonFirstName,
                SecondName = Input.ContactPersonSecondName,
                Patronymic = Input.ContactPersonPatronymic,
            };
            var languageExporter = new LanguageExporter
            {
                CommonExporter = new CommonExporter
                {
                    INN = Input.INN,
                    OGRN_IP = Input.OGRN_IP,
                    IndustryId = Input.IndustryId
                },
                Language = Languages.DefaultLanguage,
                Name = Input.Name,
                Description = Input.Description,
                ContactPersonFirstName = Input.ContactPersonFirstName,
                ContactPersonSecondName = Input.ContactPersonSecondName,
                ContactPersonPatronymic = Input.ContactPersonPatronymic,
                Position = Input.Position,
                Phone = Input.Phone,
            };

            await _context.Database.BeginTransactionAsync();
            var userCreateResult = await _userManager.CreateAsync(user, Input.Password);
            languageExporter.CommonExporter!.UserId = user.Id;

            if (userCreateResult.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");

                const string? exportersRole = Constants.ExportersRole;
                var roleAddResult = await _userManager.AddToRoleAsync(user, exportersRole);

                if (!roleAddResult.Succeeded)
                {
                    _logger.LogWarning($"Role {exportersRole} can't be added to user {user.UserName}");
                    var errorMessages = roleAddResult.Errors.Select(e => $"{e.Code}: {e.Description}");
                    _logger.LogWarning("Errors:\n" + string.Join("\n", errorMessages));
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }

                await _context.LanguageExporters!.AddAsync(languageExporter);
                await _context.SaveChangesAsync();
                _context.Database.CommitTransaction();
                if (_userManager.Options.SignIn.RequireConfirmedAccount)
                {
                    return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl });
                }

                await _signInManager.SignInAsync(user, isPersistent: false);
                return LocalRedirect(returnUrl);
            }

            foreach (var error in userCreateResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }


#nullable disable
        [BindProperty]
        public RegisterViewModel Input { get; set; }
    }
}
