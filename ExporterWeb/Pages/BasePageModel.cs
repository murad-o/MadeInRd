using ExporterWeb.Areas.Identity.Authorization;
using ExporterWeb.Helpers;
using ExporterWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace ExporterWeb.Pages
{
    public class BasePageModel : PageModel
    {
        public bool IsAdminOrManager { get; set; }
        [BindProperty(Name = "language", SupportsGet = true)]
        public string? Language { get; set; } = Languages.DefaultLanguage;

        public string? UserId { get; set; }
        public IAuthorizationService? AuthorizationService { get; set; }

        /// <summary>Initialize the UserId and IsAdminOrManager properties</summary>
        public void Init(UserManager<User> userManager)
        {
            UserId = userManager.GetUserId(User);
            IsAdminOrManager = User.IsInRole(Constants.AdministratorsRole) || User.IsInRole(Constants.ManagersRole);
        }
        public bool CanCRUD(LanguageExporter exporter) =>
            IsAdminOrManager || exporter.CommonExporterId == UserId;

        public bool CanCRUD(Product product) =>
            IsAdminOrManager || product.LanguageExporterId == UserId;

        private async Task<bool> IsAuthorized(object resource, OperationAuthorizationRequirement operation)
        {
            if (AuthorizationService is null)
                throw new NullReferenceException($"You forgot to initialize the {nameof(AuthorizationService)} property");

            var result = await AuthorizationService.AuthorizeAsync(
                User, resource, operation);
            return result.Succeeded;
        }

        protected async Task<bool> IsAuthorized(Product product, OperationAuthorizationRequirement operation) =>
            await IsAuthorized((object)product, operation);

        protected async Task<bool> IsAuthorized(LanguageExporter languageExporter, OperationAuthorizationRequirement operation) =>
            await IsAuthorized((object)languageExporter, operation);
    }
}
