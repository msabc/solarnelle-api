using Microsoft.EntityFrameworkCore;
using Solarnelle.Domain.Interfaces.DatabaseContext;
using Solarnelle.Domain.Interfaces.Repositories;
using Solarnelle.Domain.Models.Tables;

namespace Solarnelle.Infrastructure.Repositories
{
    public class UserRepository(ISolarnelleDbContext solarnelleDbContext) : IUserRepository
    {
        public async Task AddAsync(User user)
        {
            solarnelleDbContext.Users.Add(user);
            await solarnelleDbContext.SaveChangesAsync();
        }

        public async Task<User?> GetAsync(string email)
        {
            return await solarnelleDbContext.Users.SingleOrDefaultAsync(u => u.Email == email);
        }
    }
}
