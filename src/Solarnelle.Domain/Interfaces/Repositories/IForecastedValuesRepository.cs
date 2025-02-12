using Solarnelle.Domain.Enums;
using Solarnelle.Domain.Models.DatabaseResults;

namespace Solarnelle.Domain.Interfaces.Repositories
{
    public interface IForecastedValuesRepository
    {
        Task<List<PowerOutputPerSolarPowerPlantDatabaseResult>> GetForcastedTimeseriesAsync(TimeseriesGranularity granularity, DateTime dateFrom, DateTime dateTo);
    }
}
