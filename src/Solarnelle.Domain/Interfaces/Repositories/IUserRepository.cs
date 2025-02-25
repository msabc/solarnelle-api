using Microsoft.AspNetCore.Identity;

namespace Solarnelle.Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task AddAsync(IdentityUser user);

        Task<IdentityUser?> GetAsync(string email);
    }
}
