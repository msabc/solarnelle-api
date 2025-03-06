using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Solarnelle.Domain.Models.Tables;

namespace Solarnelle.Application.Services.AccessToken
{
    public interface IAccessTokenService
    {
        Task<string> CreateAccessTokenAsync(ApplicationUser user);

        Task<(ClaimsPrincipal principal, SecurityToken validatedToken)> ValidateAccessTokenAsync(string token);
    }
}
