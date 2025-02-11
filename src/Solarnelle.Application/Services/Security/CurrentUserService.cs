using System.Security.Claims;
using Solarnelle.Application.Constants;
using Solarnelle.Domain.Exceptions;

namespace Solarnelle.Application.Services.Security
{
    public class CurrentUserService : ICurrentUserService
    {
        public Guid ResolveCurrentUserId(ClaimsPrincipal principal)
        {
            var userIdClaim = principal.Claims.SingleOrDefault(x => x.Type == SecurityClaims.SolarnelleClaimsPrincipalType);

            if (userIdClaim == null || string.IsNullOrWhiteSpace(userIdClaim.Value))
                throw new CustomHttpException("Invalid request (1).", System.Net.HttpStatusCode.Unauthorized);

            if (!Guid.TryParse(userIdClaim.Value, out Guid currentUserId))
                throw new CustomHttpException("Invalid request (2).", System.Net.HttpStatusCode.Unauthorized);

            return currentUserId;
        }
    }
}
