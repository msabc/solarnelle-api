using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Solarnelle.Application.Constants;
using Solarnelle.Application.Models.Request.Auth;
using Solarnelle.Application.Services.AccessToken;
using Solarnelle.Configuration;
using Solarnelle.Configuration.Models;
using Solarnelle.Domain.Exceptions;

namespace Solarnelle.Application.Services.User
{
    public class UserService(
        UserManager<IdentityUser> userManager,
        IAccessTokenService accessTokenService) : IUserService
    {

        public async Task<string> SignUpAsync(SignUpRequest request)
        {
            var user = new IdentityUser
            {
                UserName = request.Email,
                Email = request.Email
            };

            var result = await userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
                throw new CustomHttpException("User creation failed. Please try again later.");

            return await accessTokenService.CreateAccessTokenAsync(user);
        }

        public async Task<string> SignInAsync(SignInRequest request)
        {
            var user = await userManager.FindByEmailAsync(request.Email);

            if (user is null)
                throw new CustomHttpException("No user found with this e-mail.", System.Net.HttpStatusCode.BadRequest);

            bool signInSuccessful = await userManager.CheckPasswordAsync(user, request.Password);

            if (!signInSuccessful)
                throw new CustomHttpException("Invalid credentials.", System.Net.HttpStatusCode.Unauthorized);

            return await accessTokenService.CreateAccessTokenAsync(user);
        }
    }
}