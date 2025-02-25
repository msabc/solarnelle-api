using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Solarnelle.Domain.Interfaces.DatabaseContext;
using Solarnelle.Domain.Interfaces.Repositories;

namespace Solarnelle.Infrastructure.Repositories
{
    public class UserRepository(ISolarnelleDbContext solarnelleDbContext) : IUserRepository
    {
        public async Task AddAsync(IdentityUser user)
        {
            solarnelleDbContext.Users.Add(user);

            await solarnelleDbContext.SaveChangesAsync();
        }

        public async Task<IdentityUser?> GetAsync(string email)
        {
            return await solarnelleDbContext.Users.SingleOrDefaultAsync(u => u.Email == email);
        }
    }
}
