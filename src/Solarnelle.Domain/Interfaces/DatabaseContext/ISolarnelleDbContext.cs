using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Solarnelle.Domain.Models.Tables;

namespace Solarnelle.Domain.Interfaces.DatabaseContext
{
    public interface ISolarnelleDbContext
    {
        DbSet<IdentityUser> Users { get; set; }

        DbSet<SolarPowerPlant> SolarPowerPlants { get; set; }

        DbSet<ProductionValues> ProductionValues { get; set; }

        DbSet<ForecastedValues> ForecastedValues { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
    }
}
