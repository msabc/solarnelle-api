using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Solarnelle.Application.Services.AccessToken
{
    public interface IAccessTokenService
    {
        Task<string> CreateAccessTokenAsync(IdentityUser user);

        Task<(ClaimsPrincipal principal, SecurityToken validatedToken)> ValidateAccessTokenAsync(string token);
    }
}
