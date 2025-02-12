using Solarnelle.Application.Mappers;
using Solarnelle.Application.Models.Request.PowerOutput;
using Solarnelle.Application.Models.Response.PowerOutput;
using Solarnelle.Application.Services.Validation.PowerOutput;
using Solarnelle.Domain.Enums;
using Solarnelle.Domain.Interfaces.Repositories;
using Solarnelle.Domain.Models.DatabaseResults;

namespace Solarnelle.Application.Services.PowerOutput
{
    public class PowerOutputService(
        IPowerOutputValidationService powerOutputValidationService,
        IProductionValuesRepository productionValuesRepository,
        IForecastedValuesRepository forecastedValuesRepository) : IPowerOutputService
    {
        public async Task<IEnumerable<GetPowerOutputResponse>> GetTimeseriesAsync(GetPowerOutputRequest request)
        {
            List<PowerOutputPerSolarPowerPlantDatabaseResult> databaseResults = [];

            (TimeseriesType type, TimeseriesGranularity granularity) = powerOutputValidationService.ValidateGetTimeseriesRequest(request);

            databaseResults = type switch
            {
                TimeseriesType.Production => await productionValuesRepository.GetProductionTimeseriesAsync(granularity, request.DateFrom, request.DateTo),
                TimeseriesType.Forecast => await forecastedValuesRepository.GetForcastedTimeseriesAsync(granularity, request.DateFrom, request.DateTo),
                _ => throw new NotSupportedException($"Received an unsupported {nameof(GetPowerOutputRequest.TimeseriesType)} value: {request.TimeseriesType}."),
            };

            if (databaseResults.Count == 0)
            {
                return [];
            }

            List<GetPowerOutputResponse> powerOutputsResponse = databaseResults.Select(x => x.MapToResponse()).ToList();

            return powerOutputsResponse;
        }
    }
}
