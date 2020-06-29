using ExporterWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace ExporterWeb.Areas.Identity.Authorization
{
    public class ExporterOwnerAuthorizationHandler :
        AuthorizationHandler<OperationAuthorizationRequirement, LanguageExporter>
    {
        private readonly UserManager<User> _userManager;

        public ExporterOwnerAuthorizationHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            OperationAuthorizationRequirement requirement,
            LanguageExporter languageExporter)
        {
            if (context.User is null || languageExporter is null)
                return Task.CompletedTask;

            if (requirement.Name != Constants.CreateOperationName &&
                requirement.Name != Constants.ReadOperationName &&
                requirement.Name != Constants.UpdateOperationName &&
                requirement.Name != Constants.DeleteOperationName)
            {
                return Task.CompletedTask;
            }

            if (languageExporter.CommonExporterId == _userManager.GetUserId(context.User))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
