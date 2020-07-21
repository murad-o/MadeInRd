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
        Lazy<string> _userId;
        Lazy<bool> _isAdminOrManager;

        public BasePageModel()
        {
            _userId = new Lazy<string>(() => UserManager!.GetUserId(User));
            _isAdminOrManager = new Lazy<bool>(() =>
                User.IsInRole(Constants.AdministratorsRole) || User.IsInRole(Constants.ManagersRole));
        }

        [BindProperty(Name = "language", SupportsGet = true)]
        public string? Language { get; set; } = Languages.DefaultLanguage;

        public IAuthorizationService? AuthorizationService { get; set; }

        protected UserManager<User>? UserManager { get; set; }

        public string UserId => _userId.Value;

        public bool IsAdminOrManager => _isAdminOrManager.Value;

        private async Task<bool> IsAuthorized(object resource, OperationAuthorizationRequirement operation)
        {
            if (AuthorizationService is null)
                throw new NullReferenceException($"You forgot to initialize the {nameof(AuthorizationService)} property");

            var result = await AuthorizationService.AuthorizeAsync(
                User, resource, operation);
            return result.Succeeded;
        }

        protected async Task<bool> IsAuthorized(LanguageExporter languageExporter,
            OperationAuthorizationRequirement operation) => await IsAuthorized((object)languageExporter, operation);

        protected async Task<bool> IsAuthorized(Product product,
            OperationAuthorizationRequirement operation) => await IsAuthorized((object)product, operation);
    }
}
