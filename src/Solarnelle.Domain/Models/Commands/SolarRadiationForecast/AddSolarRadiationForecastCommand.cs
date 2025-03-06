namespace Solarnelle.Domain.Models.Commands.SolarRadiationForecast
{
    public class AddSolarRadiationForecastCommand
    {
        public Guid SolarPowerPlantId { get; set; }

        public DateTime CreatedDate { get; set; }

        public float Radiation { get; set; }
    }
}
