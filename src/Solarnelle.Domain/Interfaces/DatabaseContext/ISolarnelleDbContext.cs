using Microsoft.EntityFrameworkCore;
using Solarnelle.Domain.Models.Tables;

namespace Solarnelle.Domain.Interfaces.DatabaseContext
{
    public interface ISolarnelleDbContext
    {
        DbSet<ApplicationUser> Users { get; set; }

        DbSet<SolarPowerPlant> SolarPowerPlants { get; set; }

        DbSet<ProductionValue> ProductionValues { get; set; }

        DbSet<ForecastedValue> ForecastedValues { get; set; }

        DbSet<SolarRadiationForecast> SolarRadiationForecasts { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
    }
}
