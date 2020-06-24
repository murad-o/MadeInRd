using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Threading.Tasks;

namespace ExporterWeb.Areas.Identity.Authorization
{
    public class ManagerAuthorizationHandler :
        AuthorizationHandler<OperationAuthorizationRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            OperationAuthorizationRequirement requirement)
        {
            if (context.User is { } user && user.IsInRole(Constants.ManagersRole))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
