using Microsoft.EntityFrameworkCore;
using Solarnelle.Domain.Enums;
using Solarnelle.Domain.Interfaces.DatabaseContext;
using Solarnelle.Domain.Interfaces.Repositories;
using Solarnelle.Domain.Models.DatabaseResults;

namespace Solarnelle.Infrastructure.Repositories
{
    public class ForecastedValuesRepository(ISolarnelleDbContext solarnelleDbContext) : IForecastedValuesRepository
    {
        public async Task<List<PowerOutputPerSolarPowerPlantDatabaseResult>> GetForcastedTimeseriesAsync(TimeseriesGranularity granularity, DateTime dateFrom, DateTime dateTo)
        {
            bool shouldSumWithinTheHour = granularity == TimeseriesGranularity.OneHour;

            return await (from fv in solarnelleDbContext.ForecastedValues
                          join sp in solarnelleDbContext.SolarPowerPlants
                          on fv.SolarPowerPlantId equals sp.Id
                          where fv.CreatedDate >= dateFrom && fv.CreatedDate <= dateTo
                          group fv by new
                          {
                              fv.SolarPowerPlantId,
                              sp.Name,
                              DateGroup = shouldSumWithinTheHour ? new DateTime(
                                  fv.CreatedDate.Year,
                                  fv.CreatedDate.Month,
                                  fv.CreatedDate.Day,
                                  fv.CreatedDate.Hour, 0, 0) : fv.CreatedDate
                          } into g
                          select new PowerOutputPerSolarPowerPlantDatabaseResult
                          {
                              SolarPowerPlantId = g.Key.SolarPowerPlantId,
                              Name = g.Key.Name,
                              Results = g.Select(pv => new PowerOutputDatabaseResult
                              {
                                  Date = g.Key.DateGroup,
                                  PowerOutput = shouldSumWithinTheHour ? g.Sum(p => p.PowerOutput) : pv.PowerOutput
                              }).ToList()
                          }).ToListAsync();
        }
    }
}
