using Solarnelle.Application.Models.Request.Auth;
using Solarnelle.Domain.Models.Tables;

namespace Solarnelle.Application.Mappers
{
    public static class UserMapper
    {
        public static User MapToDbObject(this SignUpRequest request, byte[] hashedPassword, byte[] passwordSalt)
        {
            return new User()
            {
                Email = request.Email,
                HashedPassword = hashedPassword,
                Salt = passwordSalt
            };
        }
    }
}
