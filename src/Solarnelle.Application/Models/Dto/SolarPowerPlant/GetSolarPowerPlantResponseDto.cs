namespace Solarnelle.Application.Models.Dto.SolarPowerPlant
{
    public class GetSolarPowerPlantResponseDto
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public DateTime DateOfInstallation { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }
    }
}
