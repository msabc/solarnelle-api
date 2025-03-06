using Microsoft.EntityFrameworkCore;
using Solarnelle.Domain.Models.Tables;

namespace Solarnelle.Infrastructure.DataSeeding
{
    public static class SolarnelleDatabaseSeeder
    {
        private static readonly int _initalSeedMinPowerOutput = 500;
        private static readonly int _initalSeedMaxPowerOutput = 1500;
        private static readonly int _initalSeedMeasurementCount = 50;
        private static readonly DateTime _initalSeedMeasurementDate = new(2023, 1, 1);
        private static readonly Random _random = new();

        public static async Task SeedDatabaseAsync(DbContext solarnelleDbContext)
        {
            var solarPowerPlants = await AddSolarPowerPlantsAsync(solarnelleDbContext);

            await AddProductionValuesAsync(solarnelleDbContext, solarPowerPlants);

            await AddForecastedValuesAsync(solarnelleDbContext, solarPowerPlants);
            
            await solarnelleDbContext.SaveChangesAsync();
        }

        public static async Task<List<SolarPowerPlant>> AddSolarPowerPlantsAsync(DbContext solarnelleDbContext)
        {
            List<SolarPowerPlant> solarPowerPlants =
            [
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Solar Plant A",
                    InstalledPower = _random.Next(_initalSeedMinPowerOutput, _initalSeedMaxPowerOutput),
                    DateOfInstallation = new DateTime(2020, 1, 15),
                    DateCreated = DateTime.UtcNow,
                    DateLastModified = DateTime.UtcNow,
                    Latitude = 48.8566m,
                    Longitude = 2.3522m
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Solar Plant B",
                    InstalledPower = _random.Next(_initalSeedMinPowerOutput, _initalSeedMaxPowerOutput),
                    DateOfInstallation = new DateTime(2021, 5, 23),
                    DateCreated = DateTime.UtcNow,
                    DateLastModified = DateTime.UtcNow,
                    Latitude = 52.5200m,
                    Longitude = 13.4050m
                },
                new ()
                {
                    Id = Guid.NewGuid(),
                    Name = "Solar Plant C",
                    InstalledPower = _random.Next(_initalSeedMinPowerOutput, _initalSeedMaxPowerOutput),
                    DateOfInstallation = new DateTime(2022, 9, 10),
                    DateCreated = DateTime.UtcNow,
                    DateLastModified = DateTime.UtcNow,
                    Latitude = 41.9028m,
                    Longitude = 12.4964m
                },
                new ()
                {
                    Id = Guid.NewGuid(),
                    Name = "Solar Plant D",
                    InstalledPower = _random.Next(_initalSeedMinPowerOutput, _initalSeedMaxPowerOutput),
                    DateOfInstallation = new DateTime(2019, 3, 5),
                    DateCreated = DateTime.UtcNow,
                    DateLastModified = DateTime.UtcNow,
                    Latitude = 51.5074m,
                    Longitude = -0.1278m
                },
                new ()
                {
                    Id = Guid.NewGuid(),
                    Name = "Solar Plant E",
                    InstalledPower = _random.Next(_initalSeedMinPowerOutput, _initalSeedMaxPowerOutput),
                    DateOfInstallation = new DateTime(2023, 11, 25),
                    DateCreated = DateTime.UtcNow,
                    DateLastModified = DateTime.UtcNow,
                    Latitude = 40.4168m,
                    Longitude = -3.7038m
                }
            ];

            await solarnelleDbContext.Set<SolarPowerPlant>().AddRangeAsync(solarPowerPlants);

            return solarPowerPlants;
        }

        private static async Task AddProductionValuesAsync(DbContext solarnelleDbContext, List<SolarPowerPlant> solarPowerPlants)
        {
            var productionValues = new List<ProductionValue>();

            foreach (var plant in solarPowerPlants)
            {
                for (int i = 0; i < _initalSeedMeasurementCount; i++)
                {
                    productionValues.Add(new ProductionValue
                    {
                        Id = Guid.NewGuid(),
                        SolarPowerPlantId = plant.Id,
                        MeasuredDate = _initalSeedMeasurementDate.AddMinutes(15 * i),
                        PowerOutput = (decimal)(_random.NextDouble() * 100)
                    });
                }
            }

            await solarnelleDbContext.Set<ProductionValue>().AddRangeAsync(productionValues);
        }

        private static async Task AddForecastedValuesAsync(DbContext solarnelleDbContext, List<SolarPowerPlant> solarPowerPlants)
        {
            var forecastedValues = new List<ForecastedValue>();

            foreach (var plant in solarPowerPlants)
            {
                for (int i = 0; i < _initalSeedMeasurementCount; i++)
                {
                    forecastedValues.Add(new ForecastedValue
                    {
                        Id = Guid.NewGuid(),
                        SolarPowerPlantId = plant.Id,
                        CreatedDate = _initalSeedMeasurementDate.AddMinutes(15 * i),
                        PowerOutput = (decimal)(_random.NextDouble() * 100)
                    });
                }
            }

            await solarnelleDbContext.Set<ForecastedValue>().AddRangeAsync(forecastedValues);
        }
    }
}
