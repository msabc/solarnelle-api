using Solarnelle.Domain.Models.Services.OpenMeteo.Dto;

namespace Solarnelle.Domain.Models.Services.OpenMeteo
{
    public class OpenMeteoRequest
    {
        public required List<OpenMeteoLocationRequestDto> Locations { get; set; }
    }
}
