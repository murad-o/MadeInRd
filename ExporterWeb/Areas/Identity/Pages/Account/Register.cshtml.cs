using ExporterWeb.Areas.Identity.Authorization;
using ExporterWeb.Helpers;
using ExporterWeb.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace ExporterWeb.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _context;

        public RegisterModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
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
            [Required(ErrorMessage = "The {0} field is required")]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; } = "";

            [Required(ErrorMessage = "The {0} field is required")]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; } = "";

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; } = "";

            // CommonExporter form
            [Required(ErrorMessage = "Field {0} is required")]
            [MaxLength(12)]
            public string INN { get; set; } = "";

            [Required(ErrorMessage = "The {0} field is required")]
            [MaxLength(15)]
            public string OGRN_IP { get; set; } = "";

            [Required(ErrorMessage = "The {0} field is required")]
            public int FieldOfActivityId { get; set; }

            // LanguageExporter form
            [Required(ErrorMessage = "The {0} field is required")]
            public string Name { get; set; } = "";

            public string? Description { get; set; }

            [Required(ErrorMessage = "The {0} field is required")]
            public string ContactPersonFirstName { get; set; } = "";
            [Required(ErrorMessage = "The {0} field is required")]
            public string ContactPersonSecondName { get; set; } = "";
            public string? ContactPersonPatronymic { get; set; }

            [Required]
            public string Position { get; set; } = "";

            [Required]
            public string Phone { get; set; } = "";
            
            [Required]
            [Range(typeof(bool), "true", "true")]
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

                await SendConfirmationEmail(user, returnUrl);
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

        private async Task SendConfirmationEmail(User user, string? returnUrl)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId = user.Id, code, returnUrl },
                protocol: Request.Scheme);

            await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
        }


#nullable disable
        [BindProperty]
        public InputModel Input { get; set; }
    }
}
