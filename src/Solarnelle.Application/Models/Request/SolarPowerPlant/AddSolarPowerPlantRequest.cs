using System.ComponentModel.DataAnnotations;

namespace Solarnelle.Application.Models.Request.SolarPowerPlant
{
    public record AddSolarPowerPlantRequest
    {
        public string? Name { get; set; }

        [Required(ErrorMessage = "Date of installation is a mandatory parameter.")]
        public DateTime DateOfInstallation { get; set; }

        [Required(ErrorMessage = "Latitude is a mandatory parameter.")]
        public decimal Latitude { get; set; }

        [Required(ErrorMessage = "Longitude is a mandatory parameter.")]
        public decimal Longitude { get; set; }
    }
}
