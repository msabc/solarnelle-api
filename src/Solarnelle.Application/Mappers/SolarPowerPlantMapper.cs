using Solarnelle.Application.Models.Dto.SolarPowerPlant;
using Solarnelle.Application.Models.Request.SolarPowerPlant;
using Solarnelle.Application.Models.Response.SolarPowerPlant;
using Solarnelle.Domain.Models.Commands.SolarPowerPlant;
using Solarnelle.Domain.Models.Tables;

namespace Solarnelle.Application.Mappers
{
    internal static class SolarPowerPlantMapper
    {
        internal static AddSolarPowerPlantCommand MapToCommand(this AddSolarPowerPlantRequest request)
        {
            return new AddSolarPowerPlantCommand()
            {
                Name = request.Name,
                DateOfInstallation = request.DateOfInstallation,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                InstalledPower = request.InstalledPower,
            };
        }

        internal static FilterSolarPowerPlantCommand MapToCommand(this GetSolarPowerPlantsRequest request)
        {
            return new FilterSolarPowerPlantCommand()
            {
                Name = request.Name,
                DateOfInstallation = request.DateOfInstallation,
                Latitude = request.Latitude,
                Longitude = request.Longitude
            };
        }

        internal static UpdateSolarPowerPlantCommand MapToCommand(this UpdateSolarPowerPlantRequest request)
        {
            return new UpdateSolarPowerPlantCommand()
            {
                Name = request.Name,
                DateOfInstallation = request.DateOfInstallation,
                Latitude = request.Latitude,
                Longitude = request.Longitude
            };
        }

        internal static GetSolarPowerPlantResponseDto MapToDto(this SolarPowerPlant solarPowerPlant)
        {
            return new GetSolarPowerPlantResponseDto()
            {
                Id = solarPowerPlant.Id,
                DateOfInstallation = solarPowerPlant.DateOfInstallation,
                Latitude = solarPowerPlant.Latitude,
                Longitude = solarPowerPlant.Longitude,
                Name = solarPowerPlant.Name
            };
        }

        internal static GetSolarPowerPlantByIdResponse MapToResponse(this SolarPowerPlant solarPowerPlant)
        {
            return new GetSolarPowerPlantByIdResponse()
            {
                Id = solarPowerPlant.Id,
                DateOfInstallation = solarPowerPlant.DateOfInstallation,
                Latitude = solarPowerPlant.Latitude,
                Longitude = solarPowerPlant.Longitude,
                Name = solarPowerPlant.Name
            };
        }
    }
}
