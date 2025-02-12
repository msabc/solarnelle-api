using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Solarnelle.Application.Constants;
using Solarnelle.Application.Mappers;
using Solarnelle.Application.Models.Request.Auth;
using Solarnelle.Configuration;
using Solarnelle.Domain.Exceptions;
using Solarnelle.Domain.Interfaces.Repositories;
using Solarnelle.Domain.Models.Tables;

namespace Solarnelle.Application.Services.Auth
{
    public class AuthService(IUserRepository userRepository) : IAuthService
    {
        public async Task<string> SignUpAsync(SignUpRequest request)
        {
            bool emailExists = await userRepository.GetAsync(request.Email) != null;

            if (emailExists)
                throw new CustomHttpException("This e-mail is already taken. Please try a different one.", System.Net.HttpStatusCode.Conflict);

            CreatePasswordHash(request.Password, out byte[] hashedPassword, out byte[] passwordSalt);
         
            User user = request.MapToDbObject(hashedPassword, passwordSalt);

            user.DateCreated = DateTime.UtcNow;
            user.Enabled = true;

            string accessToken = CreateAccessToken(user);

            await userRepository.AddAsync(user);
            
            return accessToken;
        }

        public async Task<string> SignInAsync(SignInRequest request)
        {
            var user = await userRepository.GetAsync(request.Email);

            if (user is null)
                throw new CustomHttpException("No user found with this e-mail.", System.Net.HttpStatusCode.BadRequest);

            if (!user.Enabled)
                throw new CustomHttpException("This account has been suspended.", System.Net.HttpStatusCode.Forbidden);

            if (!VerifyPasswordHash(request.Password, user.HashedPassword, user.Salt))
                throw new CustomHttpException("Invalid credentials.", System.Net.HttpStatusCode.Forbidden);

            return CreateAccessToken(user);
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        private static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }

        private static string CreateAccessToken(User user)
        {
            List<Claim> claims =
            [
                new Claim(SecurityClaims.SolarnelleClaimsPrincipalType, user.Id.ToString())
            ];

            var keyBytes = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(keyBytes);
            }

            var key = new SymmetricSecurityKey(keyBytes);

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: credentials
            ); 

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
