using Microsoft.EntityFrameworkCore;
using Solarnelle.Domain.Interfaces.DatabaseContext;
using Solarnelle.Domain.Interfaces.Repositories;
using Solarnelle.Domain.Models.DatabaseResults;

namespace Solarnelle.Infrastructure.Repositories
{
    public class ProductionValuesRepository(ISolarnelleDbContext solarnelleDbContext) : IProductionValuesRepository
    {
        public async Task<List<PowerOutputDatabaseResult>> GetAsync(DateTime dateFrom, DateTime dateTo)
        {
            return await (from pv in solarnelleDbContext.ProductionValues
                          join sp in solarnelleDbContext.SolarPowerPlants
                          on pv.SolarPowerPlantId equals sp.Id
                          where pv.MeasuredDate > dateFrom && pv.MeasuredDate < dateTo
                          select new PowerOutputDatabaseResult()
                          {
                              Id = pv.Id,
                              SolarPowerPlantId = pv.SolarPowerPlantId,
                              Name = sp.Name,
                              Date = pv.MeasuredDate,
                              PowerOutput = pv.PowerOutput
                          }).ToListAsync();
        }
    }
}
