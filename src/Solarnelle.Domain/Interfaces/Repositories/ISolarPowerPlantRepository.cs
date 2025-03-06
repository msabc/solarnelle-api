using Solarnelle.Domain.Models.Commands.SolarPowerPlant;
using Solarnelle.Domain.Models.Tables;

namespace Solarnelle.Domain.Interfaces.Repositories
{
    public interface ISolarPowerPlantRepository
    {
        Task AddAsync(AddSolarPowerPlantCommand command);

        Task<SolarPowerPlant?> GetByIdAsync(Guid id);

        Task<List<SolarPowerPlant>> GetAsReadOnlyAsync();

        Task<List<SolarPowerPlant>> GetByFilterAsync(FilterSolarPowerPlantCommand command);

        Task UpdateAsync(Guid id, UpdateSolarPowerPlantCommand command);

        Task DeleteAsync(Guid id);
    }
}
