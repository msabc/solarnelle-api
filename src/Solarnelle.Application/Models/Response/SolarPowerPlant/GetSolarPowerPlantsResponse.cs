using Solarnelle.Application.Models.Dto;

namespace Solarnelle.Application.Models.Response.SolarPowerPlant
{
    public record GetSolarPowerPlantsResponse
    {
        public required IEnumerable<GetSolarPowerPlantResponseDto> SolarPowerPlants { get; set; }
    }
}
