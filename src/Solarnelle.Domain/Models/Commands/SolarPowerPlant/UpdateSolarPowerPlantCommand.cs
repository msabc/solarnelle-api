﻿namespace Solarnelle.Domain.Models.Commands.SolarPowerPlant
{
    public class UpdateSolarPowerPlantCommand
    {
        public string? Name { get; set; }

        public DateTime DateOfInstallation { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public DateTime DateLastModified { get; set; }
    }
}
