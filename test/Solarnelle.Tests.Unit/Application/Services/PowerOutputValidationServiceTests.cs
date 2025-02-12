using Solarnelle.Application.Models.Request.PowerOutput;
using Solarnelle.Application.Services.Validation.PowerOutput;
using Solarnelle.Domain.Enums;
using Solarnelle.Domain.Exceptions;

namespace Solarnelle.Tests.Unit.Application.Services
{
    public class PowerOutputValidationServiceTests
    {
        private readonly IPowerOutputValidationService _powerOutputValidationService;

        public PowerOutputValidationServiceTests()
        {
            _powerOutputValidationService = new PowerOutputValidationService();
        }

        [Fact]
        public void ValidateGetTimeseriesRequest_DateFromIsHigherThanDateTo_ExceptionIsThrown()
        {
            var request = new GetPowerOutputRequest()
            {
                DateFrom = DateTime.UtcNow,
                DateTo = DateTime.UtcNow.AddDays(-1),
                Granularity = "forecast",
                TimeseriesType = "15 min"
            };

            var exception = Record.Exception(() => _powerOutputValidationService.ValidateGetTimeseriesRequest(request));

            Assert.NotNull(exception);
            Assert.IsType<CustomHttpException>(exception);
        }

        [Fact]
        public void ValidateGetTimeseriesRequest_InvalidTimeseries_ExceptionIsThrown()
        {
            var request = new GetPowerOutputRequest()
            {
                DateFrom = DateTime.UtcNow,
                DateTo = DateTime.UtcNow.AddDays(1),
                Granularity = "forecast",
                TimeseriesType = "something"
            };

            var exception = Record.Exception(() => _powerOutputValidationService.ValidateGetTimeseriesRequest(request));

            Assert.NotNull(exception);
            Assert.IsType<CustomHttpException>(exception);
        }

        [Fact]
        public void ValidateGetTimeseriesRequest_InvalidGranularity_ExceptionIsThrown()
        {
            var request = new GetPowerOutputRequest()
            {
                DateFrom = DateTime.UtcNow,
                DateTo = DateTime.UtcNow.AddDays(1),
                Granularity = "something",
                TimeseriesType = "15 min"
            };

            var exception = Record.Exception(() => _powerOutputValidationService.ValidateGetTimeseriesRequest(request));

            Assert.NotNull(exception);
            Assert.IsType<CustomHttpException>(exception);
        }

        [Fact]
        public void ValidateGetTimeseriesRequest_ValidRequests_ReturnParsedOutputs()
        {
            var request1 = new GetPowerOutputRequest()
            {
                DateFrom = DateTime.UtcNow,
                DateTo = DateTime.UtcNow.AddDays(1),
                TimeseriesType = "forecast",
                Granularity = "15 min",
            };

            var request2 = new GetPowerOutputRequest()
            {
                DateFrom = DateTime.UtcNow,
                DateTo = DateTime.UtcNow.AddDays(1),
                TimeseriesType = "forecast",
                Granularity = "1 hour"
            };

            var request3 = new GetPowerOutputRequest()
            {
                DateFrom = DateTime.UtcNow,
                DateTo = DateTime.UtcNow.AddDays(1),
                TimeseriesType = "production",
                Granularity = "15 min"
            };

            var request4 = new GetPowerOutputRequest()
            {
                DateFrom = DateTime.UtcNow,
                DateTo = DateTime.UtcNow.AddDays(1),
                TimeseriesType = "production",
                Granularity = "1 hour"
            };

            (TimeseriesType type1, TimeseriesGranularity granularity1) = _powerOutputValidationService.ValidateGetTimeseriesRequest(request1);
            (TimeseriesType type2, TimeseriesGranularity granularity2) = _powerOutputValidationService.ValidateGetTimeseriesRequest(request2);
            (TimeseriesType type3, TimeseriesGranularity granularity3) = _powerOutputValidationService.ValidateGetTimeseriesRequest(request3);
            (TimeseriesType type4, TimeseriesGranularity granularity4) = _powerOutputValidationService.ValidateGetTimeseriesRequest(request4);

            Assert.Equal(TimeseriesType.Forecast, type1);
            Assert.Equal(TimeseriesGranularity.FifteenMinutes, granularity1);

            Assert.Equal(TimeseriesType.Forecast, type2);
            Assert.Equal(TimeseriesGranularity.OneHour, granularity2);

            Assert.Equal(TimeseriesType.Production, type3);
            Assert.Equal(TimeseriesGranularity.FifteenMinutes, granularity3);

            Assert.Equal(TimeseriesType.Production, type4);
            Assert.Equal(TimeseriesGranularity.OneHour, granularity4);
        }
    }
}
