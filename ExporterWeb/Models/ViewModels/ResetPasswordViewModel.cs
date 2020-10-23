using System.ComponentModel.DataAnnotations;

namespace ExporterWeb.Models.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "This field is required")]
        [StringLength(100, ErrorMessage = "Passwords must consist at least {2} characters", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(Resources.Account.Account))]
        public string Password { get; set; } = "";

        [Required(ErrorMessage = "This field is required")]
        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword", ResourceType = typeof(Resources.Account.Account))]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = "";

        public string Code { get; set; } = "";
    }
}