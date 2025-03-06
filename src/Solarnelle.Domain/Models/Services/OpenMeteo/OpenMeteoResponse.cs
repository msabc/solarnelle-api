using Solarnelle.Domain.Models.Services.OpenMeteo.Dto;

namespace Solarnelle.Domain.Models.Services.OpenMeteo
{
    public record OpenMeteoResponse
    {
        public required decimal Latitude { get; set; }

        public required decimal Longitude { get; set; }

        public required OpenMeteoHourlyResponseDto Hourly { get; set; }
    }
}
