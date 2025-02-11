namespace Solarnelle.Domain.Models.Tables
{
    public class ForecastedValues
    {
        public Guid Id { get; set; }

        public Guid SolarPowerPlantId { get; set; }

        public DateTime CreatedDate { get; set; }

        public decimal PowerOutput { get; set; }
    }
}
