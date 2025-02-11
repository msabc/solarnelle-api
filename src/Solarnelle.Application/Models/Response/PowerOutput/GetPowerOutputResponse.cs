using Solarnelle.Application.Models.Dto.PowerOutput;

namespace Solarnelle.Application.Models.Response.PowerOutput
{
    public record GetPowerOutputResponse
    {
        public Guid SolarPowerPlantId { get; set; }

        public string? Name { get; set; }

        public required IEnumerable<PowerOutputResponseItemDto> GeneratedOutputs { get; set; }
    }
}
