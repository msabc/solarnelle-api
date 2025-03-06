using System.Text.Json.Serialization;

namespace Solarnelle.Domain.Models.Services.OpenMeteo.Dto
{
    public record OpenMeteoHourlyResponseDto
    {
        public required List<DateTime> Time { get; set; }

        [JsonPropertyName("temperature_2m")]
        public required List<float> Temperature { get; set; }

        [JsonPropertyName("direct_radiation")]
        public required List<float> DirectRadiation { get; set; }
    }
}
