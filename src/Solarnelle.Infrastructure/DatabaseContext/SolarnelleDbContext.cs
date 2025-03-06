using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Solarnelle.Domain.Interfaces.DatabaseContext;
using Solarnelle.Domain.Models.Tables;

namespace Solarnelle.Infrastructure.DatabaseContext
{
    public class SolarnelleDbContext(DbContextOptions<SolarnelleDbContext> options) : 
        IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>(options), ISolarnelleDbContext
    {
        public override DbSet<ApplicationUser> Users { get; set; }

        public DbSet<SolarPowerPlant> SolarPowerPlants { get; set; }

        public DbSet<ProductionValue> ProductionValues { get; set; }

        public DbSet<ForecastedValue> ForecastedValues { get; set; }

        public DbSet<SolarRadiationForecast> SolarRadiationForecasts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SolarPowerPlant>()
                .ToTable(spp => spp.HasCheckConstraint("CK_SolarPowerPlant_Latitude", "Latitude >= -90 AND Latitude <= 90"));

            modelBuilder.Entity<SolarPowerPlant>()
                .ToTable(spp => spp.HasCheckConstraint("CK_SolarPowerPlant_Longitude", "Longitude >= -180 AND Longitude <= 180"));

            modelBuilder.Entity<ProductionValue>()
                .HasOne<SolarPowerPlant>()
                .WithMany()
                .HasForeignKey(pv => pv.SolarPowerPlantId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ForecastedValue>()
                .HasOne<SolarPowerPlant>()
                .WithMany()
                .HasForeignKey(fv => fv.SolarPowerPlantId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<SolarRadiationForecast>()
                .HasOne<SolarPowerPlant>()
                .WithMany()
                .HasForeignKey(srf => srf.SolarPowerPlantId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
