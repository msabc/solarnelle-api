using Solarnelle.Application.Models.Dto.PowerOutput;
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
        public async Task<List<GetPowerOutputResponse>> GetTimeseriesAsync(GetPowerOutputRequest request)
        {
            List<PowerOutputDatabaseResult> databaseResults = [];

            (TimeseriesType type, TimeseriesGranularity granularity) = powerOutputValidationService.ValidateGetTimeseriesRequest(request);

            DateTime dateTo = request.DateTo ?? DateTime.UtcNow;

            databaseResults = type switch
            {
                TimeseriesType.Production => await productionValuesRepository.GetAsync(request.DateFrom, dateTo),
                TimeseriesType.Forecast => await forecastedValuesRepository.GetAsync(request.DateFrom, dateTo),
                _ => throw new NotSupportedException($"Received an unsupported {nameof(GetPowerOutputRequest.TimeseriesType)} value: {request.TimeseriesType}."),
            };

            if (databaseResults.Count == 0)
                return [];

            var response = new List<GetPowerOutputResponse>();

            response = granularity switch
            {
                TimeseriesGranularity.FifteenMinutes =>
                    databaseResults.GroupBy(x => new { x.SolarPowerPlantId, x.Name })
                                   .Select(g => new GetPowerOutputResponse()
                                   {
                                       SolarPowerPlantId = g.Key.SolarPowerPlantId,
                                       Name = g.Key.Name,
                                       GeneratedOutputs = g.Select(x => new PowerOutputResponseItemDto()
                                       {
                                           Date = x.Date,
                                           PowerOutput = x.PowerOutput
                                       }).OrderBy(x => x.Date).ToList()
                                   }).ToList(),

                TimeseriesGranularity.OneHour =>
                    databaseResults.GroupBy(x => new { x.SolarPowerPlantId, x.Name })
                       .Select(g => new GetPowerOutputResponse()
                       {
                           SolarPowerPlantId = g.Key.SolarPowerPlantId,
                           Name = g.Key.Name,
                           GeneratedOutputs = g.GroupBy(y => new DateTime(y.Date.Year, y.Date.Month, y.Date.Day, y.Date.Hour, 0, 0))
                                             .Select(h => new PowerOutputResponseItemDto()
                                             {
                                                 Date = h.Key,
                                                 PowerOutput = h.Sum(z => z.PowerOutput)
                                             }).OrderBy(x => x.Date).ToList()
                       }).ToList(),
                _ => throw new NotSupportedException($"Received an unsupported {nameof(GetPowerOutputRequest.Granularity)} value: {request.Granularity}.")
            };

            return response;
        }
    }
}
