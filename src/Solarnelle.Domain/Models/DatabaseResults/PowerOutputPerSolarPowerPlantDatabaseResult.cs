namespace Solarnelle.Domain.Models.DatabaseResults
{
    public class PowerOutputPerSolarPowerPlantDatabaseResult
    {
        public Guid SolarPowerPlantId { get; set; }

        public string? Name { get; set; }

        public required IEnumerable<PowerOutputDatabaseResult> Results { get; set; }
    }
}
