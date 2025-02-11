using Microsoft.EntityFrameworkCore;
using Solarnelle.Domain.Interfaces.DatabaseContext;
using Solarnelle.Domain.Interfaces.Repositories;
using Solarnelle.Domain.Models.DatabaseResults;

namespace Solarnelle.Infrastructure.Repositories
{
    public class ProductionValuesRepository(ISolarnelleDbContext solarnelleDbContext) : IProductionValuesRepository
    {
        public async Task<List<PowerOutputPerSolarPowerPlantDatabaseResult>> GetProductionTimeseriesAsync(DateTime dateFrom, DateTime dateTo)
        {
            return await (from pv in solarnelleDbContext.ProductionValues
                          join sp in solarnelleDbContext.SolarPowerPlants
                          on pv.SolarPowerPlantId equals sp.Id
                          where pv.MeasuredDate >= dateFrom && pv.MeasuredDate <= dateTo
                          group pv by new { pv.SolarPowerPlantId, sp.Name } into g
                          select new PowerOutputPerSolarPowerPlantDatabaseResult
                          {
                              SolarPowerPlantId = g.Key.SolarPowerPlantId,
                              Name = g.Key.Name,
                              Results = g.Select(pv => new PowerOutputDatabaseResult
                              {
                                  Date = pv.MeasuredDate,
                                  PowerOutput = pv.PowerOutput
                              }).ToList()
                          }).ToListAsync();
        }
    }
}
