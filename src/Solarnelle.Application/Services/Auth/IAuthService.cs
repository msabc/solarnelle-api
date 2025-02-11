using Solarnelle.Application.Models.Request.Auth;

namespace Solarnelle.Application.Services.Auth
{
    public interface IAuthService
    {
        Task<string> SignUpAsync(SignUpRequest request);

        Task<string> SignInAsync(SignInRequest request);
    }
}
