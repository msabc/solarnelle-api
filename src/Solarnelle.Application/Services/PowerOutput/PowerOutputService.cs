using Solarnelle.Application.Mappers;
using Solarnelle.Application.Models.Request.PowerOutput;
using Solarnelle.Application.Models.Response.PowerOutput;
using Solarnelle.Domain.Enums;
using Solarnelle.Domain.Exceptions;
using Solarnelle.Domain.Interfaces.Repositories;
using Solarnelle.Domain.Models.DatabaseResults;

namespace Solarnelle.Application.Services.PowerOutput
{
    public class PowerOutputService(
        IProductionValuesRepository productionValuesRepository,
        IForecastedValuesRepository forecastedValuesRepository) : IPowerOutputService
    {
        public async Task<IEnumerable<GetPowerOutputResponse>> GetTimeseriesAsync(GetPowerOutputRequest request)
        {
            List<PowerOutputPerSolarPowerPlantDatabaseResult> databaseResults = [];

            TimeseriesType timeseriesType = ParseTimeseries(request);
            TimeseriesGranularity granularity = ParseGranularity(request);

            databaseResults = timeseriesType switch
            {
                TimeseriesType.Production => await productionValuesRepository.GetProductionTimeseriesAsync(granularity, request.DateFrom, request.DateTo),
                TimeseriesType.Forecast => await forecastedValuesRepository.GetForcastedTimeseriesAsync(granularity, request.DateFrom, request.DateTo),
                _ => throw new NotSupportedException($"Received an unsupported {nameof(GetPowerOutputRequest.TimeseriesType)} value {timeseriesType}."),
            };

            if (databaseResults.Count == 0)
            {
                return [];
            }

            List<GetPowerOutputResponse> powerOutputsResponse = databaseResults.Select(x => x.MapToResponse()).ToList();

            return powerOutputsResponse;
        }

        private static TimeseriesType ParseTimeseries(GetPowerOutputRequest request)
        {
            var requestTimeseriesType = request.TimeseriesType.ToLower();

            return requestTimeseriesType switch
            {
                "production" => TimeseriesType.Production,
                "forecast" => TimeseriesType.Forecast,
                _ => throw new CustomHttpException($"Unsupported {nameof(GetPowerOutputRequest.TimeseriesType)} value provided: {requestTimeseriesType}.", System.Net.HttpStatusCode.BadRequest)
            };
        }

        private static TimeseriesGranularity ParseGranularity(GetPowerOutputRequest request)
        {
            var requestTimeseriesGranularity = request.Granularity.ToLower();

            return requestTimeseriesGranularity switch
            {
                "15 min" => TimeseriesGranularity.FifteenMinutes,
                "1 hour" => TimeseriesGranularity.OneHour,
                _ => throw new CustomHttpException($"Unsupported {nameof(GetPowerOutputRequest.Granularity)} value provided: {requestTimeseriesGranularity}.", System.Net.HttpStatusCode.BadRequest)
            };
        }
    }
}
