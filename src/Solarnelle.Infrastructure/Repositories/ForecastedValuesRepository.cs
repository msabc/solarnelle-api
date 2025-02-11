using Microsoft.EntityFrameworkCore;
using Solarnelle.Domain.Interfaces.DatabaseContext;
using Solarnelle.Domain.Interfaces.Repositories;
using Solarnelle.Domain.Models.DatabaseResults;

namespace Solarnelle.Infrastructure.Repositories
{
    public class ForecastedValuesRepository(ISolarnelleDbContext solarnelleDbContext) : IForecastedValuesRepository
    {
        public async Task<List<PowerOutputPerSolarPowerPlantDatabaseResult>> GetForcastedTimeseriesAsync(DateTime dateFrom, DateTime dateTo)
        {
            return await (from fv in solarnelleDbContext.ForecastedValues
                          join sp in solarnelleDbContext.SolarPowerPlants
                          on fv.SolarPowerPlantId equals sp.Id
                          where fv.CreatedDate >= dateFrom && fv.CreatedDate <= dateTo
                          group fv by new { fv.SolarPowerPlantId, sp.Name } into g
                          select new PowerOutputPerSolarPowerPlantDatabaseResult
                          {
                              SolarPowerPlantId = g.Key.SolarPowerPlantId,
                              Name = g.Key.Name,
                              Results = g.Select(pv => new PowerOutputDatabaseResult
                              {
                                  Date = pv.CreatedDate,
                                  PowerOutput = pv.PowerOutput
                              }).ToList()
                          }).ToListAsync();
        }
    }
}
