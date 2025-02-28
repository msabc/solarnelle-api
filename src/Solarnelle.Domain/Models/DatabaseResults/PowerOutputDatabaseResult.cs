namespace Solarnelle.Domain.Models.DatabaseResults
{
    public record PowerOutputDatabaseResult
    {
        public Guid Id { get; set; }

        public Guid SolarPowerPlantId { get; set; }

        public string? Name { get; set; }

        public DateTime Date { get; set; }

        public decimal PowerOutput { get; set; }
    }
}
