using Solarnelle.Application.Models.Request.SolarPowerPlant;
using Solarnelle.Application.Services.Validation.SolarPowerPlant;
using Solarnelle.Domain.Exceptions;

namespace Solarnelle.Tests.Unit.Application.Services
{
    public class SolarPowerPlantValidationServiceTests
    {
        private readonly ISolarPowerPlantValidationService _solarPowerPlantValidationService;

        public SolarPowerPlantValidationServiceTests()
        {
            _solarPowerPlantValidationService = new SolarPowerPlantValidationService();
        }

        [Fact]
        public void ValidateAddRequest_NameIsNull_ExceptionIsNotThrown()
        {
            var request = new AddSolarPowerPlantRequest()
            {
                DateOfInstallation = DateTime.UtcNow.AddDays(-10),
                Latitude = 50,
                Longitude = 50
            };

            var exception = Record.Exception(() => _solarPowerPlantValidationService.ValidateAddRequest(request));

            Assert.Null(exception);
        }

        [Fact]
        public void ValidateAddRequest_NameIsWhitespace_ExceptionIsThrown()
        {
            var request = new AddSolarPowerPlantRequest()
            {
                DateOfInstallation = DateTime.UtcNow.AddDays(-10),
                Latitude = 50,
                Longitude = 50,
                Name = string.Empty
            };

            var exception = Record.Exception(() => _solarPowerPlantValidationService.ValidateAddRequest(request));

            Assert.NotNull(exception);
            Assert.IsType<CustomHttpException>(exception);
        }

        [Fact]
        public void ValidateAddRequest_DateOfInstallationIsInTheFuture_ExceptionIsThrown()
        {
            var request = new AddSolarPowerPlantRequest()
            {
                DateOfInstallation = DateTime.UtcNow.AddDays(1),
                Latitude = 50,
                Longitude = 50,
                Name = "Solar power plant A"
            };

            var exception = Record.Exception(() => _solarPowerPlantValidationService.ValidateAddRequest(request));

            Assert.NotNull(exception);
            Assert.IsType<CustomHttpException>(exception);
        }

        [Fact]
        public void ValidateAddRequest_DefaultInstalledPowerValue_ExceptionIsThrown()
        {
            var request = new AddSolarPowerPlantRequest()
            {
                DateOfInstallation = DateTime.UtcNow.AddDays(-1),
                Latitude = 50,
                Longitude = 50,
                Name = "Solar power plant A",
            };

            var exception = Record.Exception(() => _solarPowerPlantValidationService.ValidateAddRequest(request));

            Assert.NotNull(exception);
            Assert.IsType<CustomHttpException>(exception);
        }
    }
}
