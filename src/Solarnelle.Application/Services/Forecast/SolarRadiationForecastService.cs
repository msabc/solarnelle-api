using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Solarnelle.Application.Mappers;
using Solarnelle.Configuration;
using Solarnelle.Domain.Interfaces.Repositories;
using Solarnelle.Domain.Interfaces.Services;
using Solarnelle.Domain.Models.Commands.SolarRadiationForecast;

namespace Solarnelle.Application.Services.Forecast
{
    public class SolarRadiationForecastService(
        ISolarPowerPlantRepository solarPowerPlantRepository,
        IOpenMeteoWeatherForecastService openMeteoWeatherForecastService,
        ISolarRadiationForecastRepository solarRadiationForecastRepository,
        IOptions<SolarnelleSettings> solarnelleOptions,
        ILogger<SolarRadiationForecastService> logger) : ISolarRadiationForecastService
    {
        private readonly int solarRadiationForecastBatchSize = solarnelleOptions.Value.SolarRadiationForecastJobSettings.SolarPowerPlantBatchSize;

        public async Task SaveSolarRadiationForecastsAsync()
        {
            if (solarRadiationForecastBatchSize <= 0)
                throw new ArgumentException($"Inalid value provided for the {nameof(SolarRadiationForecastService)} batch size.");

            var solarPowerPlants = await solarPowerPlantRepository.GetAsReadOnlyAsync();

            List<AddSolarRadiationForecastCommand> commands = [];

            var batches = solarPowerPlants
                .Select((plant, index) => new { plant, index })
                .GroupBy(x => x.index / solarRadiationForecastBatchSize)
                .Select(group => group.Select(x => x.plant).ToList());

            foreach (var batch in batches)
            {
                var plantIndexMapping = batch
                    .Select((plant, index) => new { Plant = plant, Index = index })
                    .ToList();

                var request = batch.MapToRequest();

                var forecastResponses = await openMeteoWeatherForecastService.GetWeatherForecastAsync(request);

                foreach (var mappedItem in plantIndexMapping)
                {
                    var plant = mappedItem.Plant;
                    var responseIndex = mappedItem.Index;

                    var forecastResponse = forecastResponses[responseIndex];

                    for (int k = 0; k < 24; k++)
                    {
                        commands.Add(new AddSolarRadiationForecastCommand
                        {
                            SolarPowerPlantId = plant.Id,
                            CreatedDate = forecastResponse.Hourly.Time[k],
                            Radiation = forecastResponse.Hourly.DirectRadiation[k]
                        });
                    }
                }
            }

            await solarRadiationForecastRepository.AddRangeAsync(commands);
        }
    }
}
