using Moq;
using Solarnelle.Application.Models.Request.PowerOutput;
using Solarnelle.Application.Services.PowerOutput;
using Solarnelle.Application.Services.Validation.PowerOutput;
using Solarnelle.Domain.Interfaces.Repositories;
using Solarnelle.Domain.Models.DatabaseResults;
using Solarnelle.Domain.Models.Tables;

namespace Solarnelle.Tests.Unit.Application.Services
{
    public class PowerOutputServiceTests
    {
        private readonly IPowerOutputValidationService _powerOutputValidationService;
        private readonly Mock<IProductionValuesRepository> _productionValuesRepositoryMock;
        private readonly Mock<IForecastedValuesRepository> _forecastedValuesRepositoryMock;

        private readonly IPowerOutputService _powerOutputService;

        public PowerOutputServiceTests()
        {
            _powerOutputValidationService = new PowerOutputValidationService();

            _productionValuesRepositoryMock = new Mock<IProductionValuesRepository>();
            _forecastedValuesRepositoryMock = new Mock<IForecastedValuesRepository>();

            _powerOutputService = new PowerOutputService(
               _powerOutputValidationService,
               _productionValuesRepositoryMock.Object,
               _forecastedValuesRepositoryMock.Object
            );
        }

        [Fact]
        public async Task GetTimeseriesAsync_FifteenMinutesGranularity_PowerOutputSummationIsNotPerformed()
        {
            int expectedAmountOfValues = 50;

            var request = new GetPowerOutputRequest()
            {
                DateFrom = new DateTime(2021, 1, 1),
                DateTo = new DateTime(2021, 2, 1),
                TimeseriesType = "production",
                Granularity = "15 min"
            };

            SetupPowerOutputValuesMock(10, expectedAmountOfValues, new DateTime(2021, 1, 1));

            var response = await _powerOutputService.GetTimeseriesAsync(request);

            Assert.NotNull(response);

            foreach (var item in response)
            {
                Assert.Equal(expectedAmountOfValues, item.GeneratedOutputs.Count);
            }
        }

        [Fact]
        public async Task GetTimeseriesAsync_OneHourGranularity_PowerOutputSummationIsPerformed()
        {
            int expectedAmountOfValues = 50;

            var request = new GetPowerOutputRequest()
            {
                DateFrom = new DateTime(2021, 1, 1),
                DateTo = new DateTime(2021, 2, 1),
                TimeseriesType = "production",
                Granularity = "1 hour"
            };

            SetupPowerOutputValuesMock(10, expectedAmountOfValues, new DateTime(2021, 1, 1));

            var response = await _powerOutputService.GetTimeseriesAsync(request);

            Assert.NotNull(response);

            foreach (var item in response)
            {
                Assert.Equal(4, item.GeneratedOutputs.Count);
            }
        }

        private void SetupPowerOutputValuesMock(int numberOfPowerPlants, int numberOfValues, DateTime powerOutputValuesInitialDate)
        {
            var solarPowerPlants = new List<SolarPowerPlant>();
            var powerOutputDatabaseResults = new List<PowerOutputDatabaseResult>();

            for (int i = 0; i < numberOfPowerPlants; i++)
            {
                solarPowerPlants.Add(new SolarPowerPlant()
                {
                    Id = Guid.NewGuid(),
                    Name = $"Solar Power Plant {i}"
                });
            }

            foreach (var spp in solarPowerPlants)
            {
                for (int i = 0; i < numberOfValues; i++)
                {
                    powerOutputDatabaseResults.Add(
                    new PowerOutputDatabaseResult()
                    {
                        Id = Guid.NewGuid(),
                        SolarPowerPlantId = spp.Id,
                        Name = spp.Name,
                        Date = powerOutputValuesInitialDate.AddMinutes(15),
                        PowerOutput = 10
                    });
                }
            }

            _productionValuesRepositoryMock.Setup(x => x.GetAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                                           .ReturnsAsync(powerOutputDatabaseResults);

            _forecastedValuesRepositoryMock.Setup(x => x.GetAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                                           .ReturnsAsync(powerOutputDatabaseResults);
        }
    }
}
