using System.Net.Http.Json;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Solarnelle.Configuration;
using Solarnelle.Domain.Interfaces.Services;
using Solarnelle.Domain.Models.Services.OpenMeteo;
using Solarnelle.Domain.Models.Services.WeatherForecast;

namespace Solarnelle.Infrastructure.Services.OpenMeteo
{
    public class OpenMeteoWeatherForecastService(
        ILogger<OpenMeteoWeatherForecastService> logger,
        IOptions<SolarnelleSettings> solarnelleOptions,
        HttpClient httpClient) : IOpenMeteoWeatherForecastService
    {
        private readonly string WeatherForecastPath = solarnelleOptions.Value.OpenMeteoAPISettings.GetWeatherForecastPath;

        private const string _temperatureAndDailyRadiationQueryValues = "temperature_2m,direct_radiation";

        public async Task<OpenMeteoResponse> GetWeatherForecastAsync(OpenMeteoRequest request)
        {
            try
            {
                var queryParameterStringBuilder = new StringBuilder();
                queryParameterStringBuilder.Append($"{OpenMeteoQueryParameterNames.Latitude}={request.Latitude}&");
                queryParameterStringBuilder.Append($"{OpenMeteoQueryParameterNames.Longitude}={request.Longitude}&");
                queryParameterStringBuilder.Append($"{OpenMeteoQueryParameterNames.Hourly}={_temperatureAndDailyRadiationQueryValues}&");
                queryParameterStringBuilder.Append($"{OpenMeteoQueryParameterNames.ForecastDays}=1");

                string requestUri = $"{WeatherForecastPath}?{queryParameterStringBuilder}";

                var response = await httpClient.GetFromJsonAsync<OpenMeteoResponse>(requestUri);

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
