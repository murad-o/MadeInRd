using ExporterWeb.Areas.Identity.Authorization;
using ExporterWeb.Helpers;
using ExporterWeb.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AccountResources = ExporterWeb.Resources.Account.Account;

namespace ExporterWeb.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
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

        public IList<FieldOfActivity> FieldsOfActivity
        {
            get
            {
                if (_fieldsOfActivity is null)
                    _fieldsOfActivity = _context.FieldsOfActivity.ToList();
                return _fieldsOfActivity;
            }
        }
        private IList<FieldOfActivity>? _fieldsOfActivity;

        public class InputModel
        {
            // User form 
            [Required(ErrorMessage = "This field is required")]
            [EmailAddress(ErrorMessage = "Invalid email address")]
            [Display(Name = "Email", ResourceType = typeof(AccountResources))]
            public string Email { get; set; } = "";

            [Required(ErrorMessage = "This field is required")]
            [StringLength(100, ErrorMessage = "Passwords must consist at least {2} characters", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password", ResourceType = typeof(AccountResources))]
            public string Password { get; set; } = "";

            [DataType(DataType.Password)]
            [Display(Name = "ConfirmPassword", ResourceType = typeof(AccountResources))]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            [Required(ErrorMessage = "This field is required")]
            public string ConfirmPassword { get; set; } = "";

            // CommonExporter form
            [Required(ErrorMessage = "This field is required")]
            [StringLength(10, ErrorMessage = "This field must consist {1} digits", MinimumLength = 10)]
            [Display(Name = "INN", ResourceType = typeof(AccountResources))]
            public string INN { get; set; } = "";

            [Required(ErrorMessage = "This field is required")]
            [StringLength(15, ErrorMessage = "This field must consist {2} or {1} digits", MinimumLength = 13)]
            [Display(Name = "OGRN_IP", ResourceType = typeof(AccountResources))]
            public string OGRN_IP { get; set; } = "";

            [Required(ErrorMessage = "This field is required")]
            [Display(Name = "Industry", ResourceType = typeof(AccountResources))]
            public int FieldOfActivityId { get; set; }

            // LanguageExporter form
            [Required(ErrorMessage = "This field is required")]
            [Display(Name = "Name", ResourceType = typeof(AccountResources))]
            public string Name { get; set; } = "";

            public string? Description { get; set; }

            [Required(ErrorMessage = "This field is required")]
            [Display(Name = "FirstName", ResourceType = typeof(AccountResources))]
            
            public string ContactPersonFirstName { get; set; } = "";
            [Required(ErrorMessage = "This field is required")]
            [Display(Name = "LastName", ResourceType = typeof(AccountResources))]
            public string ContactPersonSecondName { get; set; } = "";
            [Display(Name = "Patronymic", ResourceType = typeof(AccountResources))]
            public string? ContactPersonPatronymic { get; set; }

            [Required(ErrorMessage = "This field is required")]
            [Display(Name = "Position", ResourceType = typeof(AccountResources))]
            public string Position { get; set; } = "";

            [Required(ErrorMessage = "This field is required")]
            [Display(Name = "Phone", ResourceType = typeof(AccountResources))]
            [DataType(DataType.PhoneNumber)]
            [RegularExpression(@"^\+7\s\([0-9]{3}\)\s[0-9]{3}-[0-9]{2}-[0-9]{2}$", ErrorMessage = "Enter a valid phone")]
            public string Phone { get; set; } = "";

            [Required]
            public bool IsTermsOfUseAgreed { get; set; }
        }

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
                    FieldOfActivityId = Input.FieldOfActivityId
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
        public InputModel Input { get; set; }
    }
}
