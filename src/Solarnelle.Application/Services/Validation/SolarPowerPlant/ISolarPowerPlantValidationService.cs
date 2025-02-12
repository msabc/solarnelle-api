using Solarnelle.Application.Models.Request.SolarPowerPlant;

namespace Solarnelle.Application.Services.Validation.SolarPowerPlant
{
    public interface ISolarPowerPlantValidationService
    {
        void ValidateAddRequest(AddSolarPowerPlantRequest request);
    }
}
