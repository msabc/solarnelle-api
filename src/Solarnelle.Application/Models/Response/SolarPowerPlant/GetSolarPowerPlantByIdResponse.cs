using Solarnelle.Application.Models.Dto;

namespace Solarnelle.Application.Models.Response.SolarPowerPlant
{
    public record GetSolarPowerPlantByIdResponse
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public DateTime DateOfInstallation { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }
    }
}
