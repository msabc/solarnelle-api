using System.Security.Claims;

namespace Solarnelle.Application.Services.Security
{
    public interface ICurrentUserService
    {
        Guid ResolveCurrentUserId(ClaimsPrincipal principal);
    }
}
