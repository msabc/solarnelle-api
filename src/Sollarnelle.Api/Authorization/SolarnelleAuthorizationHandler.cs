using Microsoft.AspNetCore.Authorization;
using Solarnelle.Application.Constants;

namespace Solarnelle.Api.Authorization
{
    public class SolarnelleAuthorizationHandler : AuthorizationHandler<IAuthorizationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IAuthorizationRequirement requirement)
        {
            if (context.User.HasClaim(c => c.Type == SecurityClaims.SolarnelleClaimsPrincipalType))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }

}
