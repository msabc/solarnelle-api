namespace Solarnelle.Domain.Models.Services.WeatherForecast
{
    public record OpenMeteoRequest
    {
        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }
    }
}
