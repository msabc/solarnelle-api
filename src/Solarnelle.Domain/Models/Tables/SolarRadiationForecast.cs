namespace Solarnelle.Domain.Models.Tables
{
    public class SolarRadiationForecast
    {
        public Guid Id { get; set; }

        public Guid SolarPowerPlantId { get; set; }

        public DateTime CreatedDate { get; set; }

        public float Radiation { get; set; }
    }
}
