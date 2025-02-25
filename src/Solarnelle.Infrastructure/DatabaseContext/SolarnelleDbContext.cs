using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Solarnelle.Domain.Interfaces.DatabaseContext;
using Solarnelle.Domain.Models.Tables;

namespace Solarnelle.Infrastructure.DatabaseContext
{
    public class SolarnelleDbContext(DbContextOptions<SolarnelleDbContext> options) : IdentityDbContext<IdentityUser>(options), ISolarnelleDbContext
    {
        private static readonly int _initalSeedMinPowerOutput = 500;
        private static readonly int _initalSeedMaxPowerOutput = 1500;
        private static readonly int _initalSeedMeasurementCount = 50;
        private static readonly DateTime _initalSeedMeasurementDate = new(2023, 1, 1);

        public new DbSet<User> Users { get; set; }

        public DbSet<SolarPowerPlant> SolarPowerPlants { get; set; }

        public DbSet<ProductionValues> ProductionValues { get; set; }

        public DbSet<ForecastedValues> ForecastedValues { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .Property(u => u.Enabled)
                .HasDefaultValue(true);

            modelBuilder.Entity<SolarPowerPlant>()
                .ToTable(spp => spp.HasCheckConstraint("CK_SolarPowerPlant_Latitude", "Latitude >= -90 AND Latitude <= 90"));

            modelBuilder.Entity<SolarPowerPlant>()
                .ToTable(spp => spp.HasCheckConstraint("CK_SolarPowerPlant_Longitude", "Longitude >= -180 AND Longitude <= 180"));

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

            // seed data
            var random = new Random();

            var solarPowerPlants = AddSolarPowerPlants(modelBuilder, random);

            AddProductionValues(modelBuilder, random, solarPowerPlants);

            AddForecastedValues(modelBuilder, random, solarPowerPlants);
        }

        private static List<SolarPowerPlant> AddSolarPowerPlants(ModelBuilder modelBuilder, Random random)
        {
            List<SolarPowerPlant> solarPowerPlants =
            [
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Solar Plant A",
                    InstalledPower = random.Next(_initalSeedMinPowerOutput, _initalSeedMaxPowerOutput),
                    DateOfInstallation = new DateTime(2020, 1, 15),
                    Latitude = 48.8566m,
                    Longitude = 2.3522m
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Solar Plant B",
                    InstalledPower = random.Next(_initalSeedMinPowerOutput, _initalSeedMaxPowerOutput),
                    DateOfInstallation = new DateTime(2021, 5, 23),
                    Latitude = 52.5200m,
                    Longitude = 13.4050m
                },
                new ()
                {
                    Id = Guid.NewGuid(),
                    Name = "Solar Plant C",
                    InstalledPower = random.Next(_initalSeedMinPowerOutput, _initalSeedMaxPowerOutput),
                    DateOfInstallation = new DateTime(2022, 9, 10),
                    Latitude = 41.9028m,
                    Longitude = 12.4964m
                },
                new ()
                {
                    Id = Guid.NewGuid(),
                    Name = "Solar Plant D",
                    InstalledPower = random.Next(_initalSeedMinPowerOutput, _initalSeedMaxPowerOutput),
                    DateOfInstallation = new DateTime(2019, 3, 5),
                    Latitude = 51.5074m,
                    Longitude = -0.1278m
                },
                new ()
                {
                    Id = Guid.NewGuid(),
                    Name = "Solar Plant E",
                    InstalledPower = random.Next(_initalSeedMinPowerOutput, _initalSeedMaxPowerOutput),
                    DateOfInstallation = new DateTime(2023, 11, 25),
                    Latitude = 40.4168m,
                    Longitude = -3.7038m
                }
            ];

            modelBuilder.Entity<SolarPowerPlant>().HasData(solarPowerPlants);

            return solarPowerPlants;
        }

        private static void AddProductionValues(ModelBuilder modelBuilder, Random random, IEnumerable<SolarPowerPlant> solarPowerPlants)
        {
            var productionValues = new List<ProductionValues>();

            foreach (var plant in solarPowerPlants)
            {
                for (int i = 0; i < _initalSeedMeasurementCount; i++)
                {
                    productionValues.Add(new ProductionValues
                    {
                        Id = Guid.NewGuid(),
                        SolarPowerPlantId = plant.Id,
                        MeasuredDate = _initalSeedMeasurementDate.AddMinutes(15 * i),
                        PowerOutput = (decimal)(random.NextDouble() * 100)
                    });
                }
            }

            modelBuilder.Entity<ProductionValues>().HasData(productionValues);
        }

        private static void AddForecastedValues(ModelBuilder modelBuilder, Random random, IEnumerable<SolarPowerPlant> solarPowerPlants)
        {
            var forecastedValues = new List<ForecastedValues>();

            foreach (var plant in solarPowerPlants)
            {
                for (int i = 0; i < _initalSeedMeasurementCount; i++)
                {
                    forecastedValues.Add(new ForecastedValues
                    {
                        Id = Guid.NewGuid(),
                        SolarPowerPlantId = plant.Id,
                        CreatedDate = _initalSeedMeasurementDate.AddMinutes(15 * i),
                        PowerOutput = (decimal)(random.NextDouble() * 100)
                    });
                }
            }

            modelBuilder.Entity<ForecastedValues>().HasData(forecastedValues);
        }
    }
}
