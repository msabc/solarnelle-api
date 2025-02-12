using Microsoft.EntityFrameworkCore;
using Solarnelle.Domain.Enums;
using Solarnelle.Domain.Interfaces.DatabaseContext;
using Solarnelle.Domain.Interfaces.Repositories;
using Solarnelle.Domain.Models.DatabaseResults;

namespace Solarnelle.Infrastructure.Repositories
{
    public class ProductionValuesRepository(ISolarnelleDbContext solarnelleDbContext) : IProductionValuesRepository
    {
        public async Task<List<PowerOutputPerSolarPowerPlantDatabaseResult>> GetProductionTimeseriesAsync(TimeseriesGranularity granularity, DateTime dateFrom, DateTime dateTo)
        {
            bool shouldSumWithinTheHour = granularity == TimeseriesGranularity.OneHour;

            return await (from pv in solarnelleDbContext.ProductionValues
                          join sp in solarnelleDbContext.SolarPowerPlants
                          on pv.SolarPowerPlantId equals sp.Id
                          where pv.MeasuredDate >= dateFrom && pv.MeasuredDate <= dateTo
                          group pv by new
                          {
                              pv.SolarPowerPlantId,
                              sp.Name,
                              DateGroup = shouldSumWithinTheHour ? new DateTime(
                                  pv.MeasuredDate.Year,
                                  pv.MeasuredDate.Month,
                                  pv.MeasuredDate.Day,
                                  pv.MeasuredDate.Hour, 0, 0) : pv.MeasuredDate
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
