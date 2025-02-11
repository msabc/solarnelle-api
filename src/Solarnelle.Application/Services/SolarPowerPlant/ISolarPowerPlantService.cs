using Solarnelle.Application.Models.Request.SolarPowerPlant;
using Solarnelle.Application.Models.Response.SolarPowerPlant;

namespace Solarnelle.Application.Services.SolarPowerPlant
{
    public interface ISolarPowerPlantService
    {
        Task AddAsync(Guid userId, AddSolarPowerPlantRequest request);

        Task<GetSolarPowerPlantsResponse> GetAsync(GetSolarPowerPlantsRequest request);
        
        Task<GetSolarPowerPlantByIdResponse?> GetByIdAsync(Guid id);

        Task UpdateAsync(Guid id, UpdateSolarPowerPlantRequest request);

        Task DeleteAsync(Guid id);
    }
}
