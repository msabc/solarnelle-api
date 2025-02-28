using Solarnelle.Domain.Enums;
using Solarnelle.Domain.Models.DatabaseResults;

namespace Solarnelle.Domain.Interfaces.Repositories
{
    public interface IProductionValuesRepository
    {
        Task<List<PowerOutputDatabaseResult>> GetAsync(DateTime dateFrom, DateTime dateTo);
    }
}
