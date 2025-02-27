using Solarnelle.Application.Models.Request.SolarPowerPlant;
using Solarnelle.Domain.Exceptions;

namespace Solarnelle.Application.Services.Validation.SolarPowerPlant
{
    public class SolarPowerPlantValidationService : ISolarPowerPlantValidationService
    {
        public void ValidateAddRequest(AddSolarPowerPlantRequest request)
        {
            if (request.Name is not null && string.IsNullOrWhiteSpace(request.Name))
                throw new CustomHttpException("Name cannot be empty. Please enter a valid name.", System.Net.HttpStatusCode.BadRequest);

            if (request.DateOfInstallation > DateTime.UtcNow)
                throw new CustomHttpException("Date of installation cannot be in the future. Please enter a valid date.", System.Net.HttpStatusCode.BadRequest);

            if (request.InstalledPower <= 0)
                throw new CustomHttpException("Installed power cannot be less than 0. Please enter a value higher than 0.", System.Net.HttpStatusCode.BadRequest);
        }
    }
}
