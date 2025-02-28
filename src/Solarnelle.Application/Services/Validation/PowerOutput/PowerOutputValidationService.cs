using Solarnelle.Application.Models.Request.PowerOutput;
using Solarnelle.Domain.Enums;
using Solarnelle.Domain.Exceptions;

namespace Solarnelle.Application.Services.Validation.PowerOutput
{
    public class PowerOutputValidationService : IPowerOutputValidationService
    {
        public (TimeseriesType, TimeseriesGranularity) ValidateGetTimeseriesRequest(GetPowerOutputRequest request)
        {
            if (request.DateTo.HasValue && request.DateFrom >= request.DateTo)
                throw new CustomHttpException($"'Date from' needs to be before 'date to'.", System.Net.HttpStatusCode.BadRequest);

            TimeseriesType type = TimeseriesType.Production;
            TimeseriesGranularity granularity = TimeseriesGranularity.FifteenMinutes;

            if (!string.IsNullOrWhiteSpace(request.TimeseriesType))
            {
                var requestTimeseriesType = request.TimeseriesType.ToLower();

                type = requestTimeseriesType switch
                {
                    "production" => TimeseriesType.Production,
                    "forecast" => TimeseriesType.Forecast,
                    _ => throw new CustomHttpException($"Unsupported {nameof(GetPowerOutputRequest.TimeseriesType)} value provided: {requestTimeseriesType}.", System.Net.HttpStatusCode.BadRequest)
                };
            }

            if (!string.IsNullOrWhiteSpace(request.Granularity))
            {
                var requestTimeseriesGranularity = request.Granularity.ToLower();

                granularity = requestTimeseriesGranularity switch
                {
                    "15 min" => TimeseriesGranularity.FifteenMinutes,
                    "1 hour" => TimeseriesGranularity.OneHour,
                    _ => throw new CustomHttpException($"Unsupported {nameof(GetPowerOutputRequest.Granularity)} value provided: {requestTimeseriesGranularity}.", System.Net.HttpStatusCode.BadRequest)
                };
            }

            return (type, granularity);
        }
    }
}
