using Solarnelle.Domain.Models.Tables;

namespace Solarnelle.Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task AddAsync(User user);

        Task<User?> GetAsync(string email);
    }
}
