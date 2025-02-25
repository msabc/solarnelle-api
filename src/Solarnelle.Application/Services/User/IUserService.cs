using Solarnelle.Application.Models.Request.Auth;

namespace Solarnelle.Application.Services.User
{
    public interface IUserService
    {
        Task<string> SignUpAsync(SignUpRequest request);

        Task<string> SignInAsync(SignInRequest request);
    }
}
