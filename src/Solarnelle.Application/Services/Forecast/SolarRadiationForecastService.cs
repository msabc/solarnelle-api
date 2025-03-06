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
            var solarPowerPlants = await solarPowerPlantRepository.GetAsReadOnlyAsync();

            List<AddSolarRadiationForecastCommand> commands = [];

            for (int i = 0; i < solarPowerPlants.Count; i += solarRadiationForecastBatchSize)
            {
                var solarPowerPlantsBatch = solarPowerPlants.Skip(i).Take(solarRadiationForecastBatchSize).ToList();

                var request = solarPowerPlantsBatch.MapToRequest();

                var forecastResponses = await openMeteoWeatherForecastService.GetWeatherForecastAsync(request);

                for (int j = 0; j < solarRadiationForecastBatchSize; j++)
                {
                    for (int k = 0; k < 24; k++)
                    {
                        commands.Add(new AddSolarRadiationForecastCommand()
                        {
                            SolarPowerPlantId = solarPowerPlantsBatch[i].Id,
                            CreatedDate = forecastResponses[j].Hourly.Time[k],
                            Radiation = forecastResponses[j].Hourly.DirectRadiation[k]
                        });
                    }
                }
            }

            await solarRadiationForecastRepository.AddRangeAsync(commands);
        }
    }
}
