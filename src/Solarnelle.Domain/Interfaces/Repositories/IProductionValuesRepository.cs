using Solarnelle.Domain.Enums;
using Solarnelle.Domain.Models.DatabaseResults;

namespace Solarnelle.Domain.Interfaces.Repositories
{
    public interface IProductionValuesRepository
    {
        Task<List<PowerOutputPerSolarPowerPlantDatabaseResult>> GetProductionTimeseriesAsync(TimeseriesGranularity granularity, DateTime from, DateTime to);
    }
}
