using System.ComponentModel.DataAnnotations;

namespace ExporterWeb.Models.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "This field is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name = "Email", ResourceType = typeof(Resources.Account.Account))]
        public string Email { get; set; } = "";
    }
}