using Solarnelle.Domain.Models.Services.OpenMeteo;
using Solarnelle.Domain.Models.Services.WeatherForecast;

namespace Solarnelle.Domain.Interfaces.Services
{
    public interface IOpenMeteoWeatherForecastService
    {
        Task<OpenMeteoResponse> GetWeatherForecastAsync(OpenMeteoRequest request);
    }
}
