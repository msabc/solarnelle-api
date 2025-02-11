namespace Solarnelle.Domain.Models.Commands
{
    public class AddSolarPowerPlantCommand
    {
        public string? Name { get; set; }

        public DateTime DateOfInstallation { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public Guid UserId { get; set; }
    }
}
