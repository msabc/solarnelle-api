using Solarnelle.Domain.Models.Commands.SolarRadiationForecast;

namespace Solarnelle.Domain.Interfaces.Repositories
{
    public interface ISolarRadiationForecastRepository
    {
        Task AddRangeAsync(List<AddSolarRadiationForecastCommand> commands);
    }
}
