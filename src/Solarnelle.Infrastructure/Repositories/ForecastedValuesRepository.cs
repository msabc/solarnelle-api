using Microsoft.EntityFrameworkCore;
using Solarnelle.Domain.Interfaces.DatabaseContext;
using Solarnelle.Domain.Interfaces.Repositories;
using Solarnelle.Domain.Models.DatabaseResults;

namespace Solarnelle.Infrastructure.Repositories
{
    public class ForecastedValuesRepository(ISolarnelleDbContext solarnelleDbContext) : IForecastedValuesRepository
    {
        public async Task<List<PowerOutputDatabaseResult>> GetAsync(DateTime dateFrom, DateTime dateTo)
        {
            return await (from fv in solarnelleDbContext.ForecastedValues
                          join sp in solarnelleDbContext.SolarPowerPlants
                          on fv.SolarPowerPlantId equals sp.Id
                          where fv.CreatedDate > dateFrom && fv.CreatedDate < dateTo
                          select new PowerOutputDatabaseResult()
                          {
                              Id = fv.Id,
                              SolarPowerPlantId = fv.SolarPowerPlantId,
                              Name = sp.Name,
                              Date = fv.CreatedDate,
                              PowerOutput = fv.PowerOutput
                          }).ToListAsync();
        }
    }
}
