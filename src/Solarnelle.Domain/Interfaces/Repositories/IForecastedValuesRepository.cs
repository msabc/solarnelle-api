using Solarnelle.Domain.Models.DatabaseResults;

namespace Solarnelle.Domain.Interfaces.Repositories
{
    public interface IForecastedValuesRepository
    {
        Task<List<PowerOutputPerSolarPowerPlantDatabaseResult>> GetForcastedTimeseriesAsync(DateTime dateFrom, DateTime dateTo);
    }
}
