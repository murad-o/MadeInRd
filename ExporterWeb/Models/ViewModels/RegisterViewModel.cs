using System.ComponentModel.DataAnnotations;
using AccountResources = ExporterWeb.Resources.Account.Account;

namespace ExporterWeb.Models.ViewModels
{
    public class RegisterViewModel
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
            [RegularExpression(@"^(\d{10}|\d{12})$", ErrorMessage = "This field must consist 10 or 12 digits")]
            [StringLength(12, MinimumLength = 10)]
            [Display(Name = "INN", ResourceType = typeof(AccountResources))]
            public string INN { get; set; } = "";

            [Required(ErrorMessage = "This field is required")]
            [RegularExpression(@"^(\d{13}|\d{15})$", ErrorMessage = "This field must consist 13 or 15 digits")]
            [StringLength(15, MinimumLength = 13)]
            [Display(Name = "OGRN_IP", ResourceType = typeof(AccountResources))]
            public string OGRN_IP { get; set; } = "";

            [Required(ErrorMessage = "This field is required")]
            [Display(Name = "Industry", ResourceType = typeof(AccountResources))]
            public int IndustryId { get; set; }

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
            [RegularExpression(@"^\+7\s\(\d{3}\)\s\d{3}-\d{2}-\d{2}$", ErrorMessage = "Enter a valid phone")]
            public string Phone { get; set; } = "";

            [Required]
            [Range(typeof(bool), "true", "true", ErrorMessage = "Your consent with the user agreement is required")]
            public bool IsTermsOfUseAgreed { get; set; }
    }
}