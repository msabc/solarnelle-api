namespace Solarnelle.Domain.Models.Tables
{
    public class SolarPowerPlant
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public DateTime DateOfInstallation { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public Guid CreatedByUserId { get; set; }

        public Guid LastModifiedUserId { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateLastModified { get; set; }
    }
}
