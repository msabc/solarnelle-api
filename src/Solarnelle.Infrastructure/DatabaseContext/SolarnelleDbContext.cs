using Microsoft.EntityFrameworkCore;
using Solarnelle.Domain.Interfaces.DatabaseContext;
using Solarnelle.Domain.Models.Tables;

namespace Solarnelle.Infrastructure.DatabaseContext
{
    public class SolarnelleDbContext(DbContextOptions<SolarnelleDbContext> options) : DbContext(options), ISolarnelleDbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<SolarPowerPlant> SolarPowerPlants { get; set; }

        public DbSet<ProductionValues> ProductionValues { get; set; }

        public DbSet<ForecastedValues> ForecastedValues { get; set; }

        public DbSet<ForecastGranularity> ForecastGranularities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(u => u.Enabled)
                .HasDefaultValue(true);

            modelBuilder.Entity<SolarPowerPlant>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(spp => spp.CreatedByUserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ProductionValues>()
                .HasOne<SolarPowerPlant>()
                .WithMany()
                .HasForeignKey(pv => pv.SolarPowerPlantId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ForecastedValues>()
                .HasOne<SolarPowerPlant>()
                .WithMany()
                .HasForeignKey(fv => fv.SolarPowerPlantId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ForecastedValues>()
                .HasOne(fv => fv.Granularity)
                .WithMany()
                .HasForeignKey(fv => fv.GranularityId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ForecastGranularity>().HasData(
                new ForecastGranularity { Id = 1, Name = "15min" },
                new ForecastGranularity { Id = 2, Name = "1hour" }
            );
        }
    }
}
