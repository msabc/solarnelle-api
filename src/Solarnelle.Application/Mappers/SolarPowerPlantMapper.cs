﻿using Solarnelle.Application.Models.Dto;
using Solarnelle.Application.Models.Request.SolarPowerPlant;
using Solarnelle.Application.Models.Response.SolarPowerPlant;
using Solarnelle.Domain.Models.Commands;
using Solarnelle.Domain.Models.Tables;

namespace Solarnelle.Application.Mappers
{
    public static class SolarPowerPlantMapper
    {
        public static AddSolarPowerPlantCommand MapToCommand(this AddSolarPowerPlantRequest request, Guid userId)
        {
            return new AddSolarPowerPlantCommand()
            {
                Name = request.Name,
                DateOfInstallation = request.DateOfInstallation,
                Latitude = request.Latitude,
                Longitude = request.Longitude
            };
        }

        public static FilterSolarPowerPlantCommand MapToCommand(this GetSolarPowerPlantsRequest request)
        {
            return new FilterSolarPowerPlantCommand()
            {
                Name = request.Name,
                DateOfInstallation = request.DateOfInstallation,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                CreatedByUserId = request.CreatedByUserId
            };
        }

        public static UpdateSolarPowerPlantCommand MapToCommand(this UpdateSolarPowerPlantRequest request)
        {
            return new UpdateSolarPowerPlantCommand()
            {
                Name = request.Name,
                DateOfInstallation = request.DateOfInstallation,
                Latitude = request.Latitude,
                Longitude = request.Longitude
            };
        }

        public static GetSolarPowerPlantResponseDto MapToDto(this SolarPowerPlant solarPowerPlant)
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

        public static GetSolarPowerPlantByIdResponse MapToResponse(this SolarPowerPlant solarPowerPlant)
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
