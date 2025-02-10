namespace Solarnelle.Domain.Models.Tables
{
    public class ForecastedValues
    {
        public Guid Id { get; set; }

        public Guid SolarPowerPlantId { get; set; }

        public DateTime CreatedDate { get; set; }

        public decimal Output { get; set; }

        public int GranularityId { get; set; }

        public required ForecastGranularity Granularity { get; set; }
    }
}
