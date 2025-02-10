namespace Solarnelle.Domain.Models.Tables
{
    public class ProductionValues
    {
        public Guid Id { get; set; }

        public Guid SolarPowerPlantId { get; set; }

        public required SolarPowerPlant SolarPowerPlant { get; set; }

        public DateTime MeasuredDate { get; set; }

        public decimal Output { get; set; }
    }
}
