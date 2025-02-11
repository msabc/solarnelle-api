using Solarnelle.Domain.Models.DatabaseResults;

namespace Solarnelle.Domain.Interfaces.Repositories
{
    public interface IProductionValuesRepository
    {
        Task<List<PowerOutputPerSolarPowerPlantDatabaseResult>> GetProductionTimeseriesAsync(DateTime from, DateTime to);
    }
}
