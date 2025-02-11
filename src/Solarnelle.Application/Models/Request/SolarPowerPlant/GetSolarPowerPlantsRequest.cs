namespace Solarnelle.Application.Models.Request.SolarPowerPlant
{
    public record GetSolarPowerPlantsRequest
    {
        public Guid? Id { get; set; }

        public string? Name { get; set; }

        public DateTime? DateOfInstallation { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        public Guid? CreatedByUserId { get; set; }
    }
}
