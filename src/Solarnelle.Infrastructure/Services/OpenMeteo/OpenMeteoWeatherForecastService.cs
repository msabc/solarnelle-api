using System.Net.Http.Json;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Solarnelle.Configuration;
using Solarnelle.Domain.Interfaces.Services;
using Solarnelle.Domain.Models.Services.OpenMeteo;

namespace Solarnelle.Infrastructure.Services.OpenMeteo
{
    public class OpenMeteoWeatherForecastService(
        ILogger<OpenMeteoWeatherForecastService> logger,
        IOptions<SolarnelleSettings> solarnelleOptions,
        HttpClient httpClient) : IOpenMeteoWeatherForecastService
    {
        private readonly string WeatherForecastPath = solarnelleOptions.Value.OpenMeteoAPISettings.GetWeatherForecastPath;

        private const string TemperatureAndDirectRadiationQueryValues = "temperature_2m,direct_radiation";

        public async Task<List<OpenMeteoResponse>> GetWeatherForecastAsync(OpenMeteoRequest request)
        {
            try
            {
                string latitudes = string.Join(",", request.Locations.Select(x => Math.Round(x.Latitude, 2).ToString()));
                string longitudes = string.Join(",", request.Locations.Select(x => Math.Round(x.Longitude, 2).ToString()));
                
                var queryParameterStringBuilder = new StringBuilder();
                
                queryParameterStringBuilder.Append($"{OpenMeteoQueryParameterNames.Latitude}={latitudes}&");
                queryParameterStringBuilder.Append($"{OpenMeteoQueryParameterNames.Longitude}={longitudes}&");
                queryParameterStringBuilder.Append($"{OpenMeteoQueryParameterNames.Hourly}={TemperatureAndDirectRadiationQueryValues}&");
                queryParameterStringBuilder.Append($"{OpenMeteoQueryParameterNames.ForecastDays}=1");

                string requestUri = $"{WeatherForecastPath}?{queryParameterStringBuilder}";

                var response = await httpClient.GetFromJsonAsync<List<OpenMeteoResponse>>(requestUri);

                return response!;
            }
            catch (Exception ex)
            {
                logger.LogError($"{nameof(OpenMeteoWeatherForecastService)}.{nameof(GetWeatherForecastAsync)} error occurred: {ex.Message}");
                throw;
            }
        }
    }
}
