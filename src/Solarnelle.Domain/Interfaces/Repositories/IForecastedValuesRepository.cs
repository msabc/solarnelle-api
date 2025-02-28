using Solarnelle.Domain.Models.DatabaseResults;

namespace Solarnelle.Domain.Interfaces.Repositories
{
    public interface IForecastedValuesRepository
    {
        Task<List<PowerOutputDatabaseResult>> GetAsync(DateTime dateFrom, DateTime dateTo);
    }
}
