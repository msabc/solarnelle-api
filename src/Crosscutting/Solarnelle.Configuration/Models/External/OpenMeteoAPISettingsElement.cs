namespace Solarnelle.Configuration.Models.External
{
    public record OpenMeteoAPISettingsElement
    {
        public required string BaseURL { get; set; }

        public required string GetWeatherForecastPath { get; set; }
    }
}
