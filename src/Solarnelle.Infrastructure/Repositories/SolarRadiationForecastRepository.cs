using Solarnelle.Domain.Interfaces.DatabaseContext;
using Solarnelle.Domain.Interfaces.Repositories;
using Solarnelle.Domain.Models.Commands.SolarRadiationForecast;
using Solarnelle.Domain.Models.Tables;

namespace Solarnelle.Infrastructure.Repositories
{
    public class SolarRadiationForecastRepository(ISolarnelleDbContext solarnelleDbContext) : ISolarRadiationForecastRepository
    {
        public async Task AddRangeAsync(List<AddSolarRadiationForecastCommand> commands)
        {
            List<SolarRadiationForecast> solarRadiationForecasts = commands.Select(x => new SolarRadiationForecast()
            {
                SolarPowerPlantId = x.SolarPowerPlantId,
                CreatedDate = x.CreatedDate,
                Radiation = x.Radiation
            }).ToList();

            await solarnelleDbContext.SolarRadiationForecasts.AddRangeAsync(solarRadiationForecasts);

            await solarnelleDbContext.SaveChangesAsync();
        }
    }
}
