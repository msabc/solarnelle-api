﻿using Solarnelle.Application.Models.Dto.PowerOutput;
using Solarnelle.Application.Models.Response.PowerOutput;
using Solarnelle.Domain.Models.DatabaseResults;

namespace Solarnelle.Application.Mappers
{
    internal static class PowerOutputMapper
    {
        internal static GetPowerOutputResponse MapToResponse(this PowerOutputPerSolarPowerPlantDatabaseResult result)
        {
            return new GetPowerOutputResponse()
            {
                SolarPowerPlantId = result.SolarPowerPlantId,
                Name = result.Name,
                GeneratedOutputs = result.Results.Select(MapToDto).ToList(),
            };
        }

        internal static PowerOutputResponseItemDto MapToDto(this PowerOutputDatabaseResult result)
        {
            return new PowerOutputResponseItemDto()
            {
                Date = result.Date,
                PowerOutput = result.PowerOutput,
            };
        }
    }
}
