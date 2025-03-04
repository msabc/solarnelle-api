﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Solarnelle.Application.Models.Request.SolarPowerPlant
{
    public record AddSolarPowerPlantRequest
    {
        public string? Name { get; set; }

        [Required(ErrorMessage = "Date of installation is a mandatory parameter.")]
        public DateTime DateOfInstallation { get; set; }

        [Required(ErrorMessage = "Latitude is a mandatory parameter.")]
        [Range(-90, 90, ErrorMessage = "Latitude parameter value must be between -90 and 90 degrees.")]
        public decimal Latitude { get; set; }

        [Required(ErrorMessage = "Longitude is a mandatory parameter.")]
        [Range(-180, 180, ErrorMessage = "Longitude parameter value must be between -180 and 180 degrees.")]
        public decimal Longitude { get; set; }

        [Description("Represents the installed power of a solar power plant measured in megawatts (MW).")]
        [Range(0, double.MaxValue, ErrorMessage = "Installed power value cannot be less than 0.")]
        public decimal InstalledPower {  get; set; }
    }
}
