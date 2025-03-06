namespace Solarnelle.Domain.Models.Tables
{
    public class ForecastedValue
    {
        public Guid Id { get; set; }

        public Guid SolarPowerPlantId { get; set; }

        public DateTime CreatedDate { get; set; }

        public decimal PowerOutput { get; set; }
    }
}
