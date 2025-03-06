using Solarnelle.Domain.Models.Services.OpenMeteo;

namespace Solarnelle.Domain.Interfaces.Services
{
    public interface IOpenMeteoWeatherForecastService
    {
        Task<List<OpenMeteoResponse>> GetWeatherForecastAsync(OpenMeteoRequest request);
    }
}
