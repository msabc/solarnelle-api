using Solarnelle.Domain.Models.Services.OpenMeteo;
using Solarnelle.Domain.Models.Services.OpenMeteo.Dto;
using Solarnelle.Domain.Models.Tables;

namespace Solarnelle.Application.Mappers
{
    internal static class SolarRadiationForecastMapper
    {
        internal static OpenMeteoRequest MapToRequest(this IEnumerable<SolarPowerPlant> solarPowerPlants)
        {
            OpenMeteoRequest request = new()
            {
                Locations = []
            };

            foreach (var spp in solarPowerPlants) {
                request.Locations.Add(new OpenMeteoLocationRequestDto() { 
                    Latitude = spp.Latitude,
                    Longitude = spp.Longitude
                });
            }

            return request;
        }
    }
}
