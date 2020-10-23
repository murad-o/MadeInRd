using System.ComponentModel.DataAnnotations;

namespace ExporterWeb.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "This field is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name = "Email", ResourceType = typeof(Resources.Account.Account))]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "This field is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(Resources.Account.Account))]
        public string Password { get; set; } = "";
    }
}