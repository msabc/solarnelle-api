using System.Text.Json.Serialization;

namespace Solarnelle.Domain.Models.Services.OpenMeteo.Dto
{
    public record OpenMeteoHourlyDto
    {
        public required IEnumerable<DateTime> Time { get; set; }

        [JsonPropertyName("temperature_2m")]
        public required IEnumerable<float> Temperature { get; set; }

        [JsonPropertyName("direct_radiation")]
        public required IEnumerable<float> DirectRadiation { get; set; }
    }
}
