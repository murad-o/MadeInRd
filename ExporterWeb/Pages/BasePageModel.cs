using ExporterWeb.Areas.Identity.Authorization;
using ExporterWeb.Helpers;
using ExporterWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ExporterWeb.Pages
{
    public class BasePageModel : PageModel
    {
        public bool IsAdminOrManager { get; set; }
        [BindProperty(Name = "language", SupportsGet = true)]
        public string? Language { get; set; } = Languages.DefaultLanguage;

        public string? UserId { get; set; }

        /// <summary>Initialize the UserId and IsAdminOrManager properties</summary>
        public void Init(UserManager<User> userManager)
        {
            UserId = userManager.GetUserId(User);
            IsAdminOrManager = User.IsInRole(Constants.AdministratorsRole) || User.IsInRole(Constants.ManagersRole);
        }
        public bool CanCRUD(LanguageExporter exporter)
        {
            if (IsAdminOrManager || exporter.CommonExporterId == UserId)
                return true;
            return false;
        }

        public bool CanCRUD(Product product)
        {
            if (IsAdminOrManager || product.LanguageExporterId == UserId)
                return true;
            return false;
        }
    }
}
