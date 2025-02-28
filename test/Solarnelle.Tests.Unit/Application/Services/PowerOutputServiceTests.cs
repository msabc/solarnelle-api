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
            int numberOfPowerPlants = 10;
            int expectedAmountOfValuesPerPowerPlant = 50;
            DateTime startingDateTime = new(2021, 1, 1);
            int numberOfMinutesBetweenEachValue = 15;

            var request = new GetPowerOutputRequest()
            {
                DateFrom = startingDateTime,
                DateTo = new DateTime(2021, 2, 1),
                TimeseriesType = "production",
                Granularity = "15 min"
            };

            SetupPowerOutputValuesMock(
                numberOfPowerPlants, 
                expectedAmountOfValuesPerPowerPlant,
                startingDateTime, 
                numberOfMinutesBetweenEachValue
            );

            var response = await _powerOutputService.GetTimeseriesAsync(request);

            Assert.NotNull(response);

            foreach (var item in response)
            {
                Assert.Equal(expectedAmountOfValuesPerPowerPlant, item.GeneratedOutputs.Count);
            }
        }

        [Fact]
        public async Task GetTimeseriesAsync_OneHourGranularity_PowerOutputSummationIsPerformed()
        {
            int numberOfPowerPlants = 1;
            int expectedAmountOfValuesPerPowerPlant = 50;
            DateTime startingDateTime = new(2021, 1, 1);
            int numberOfMinutesBetweenEachValue = 15;

            var request = new GetPowerOutputRequest()
            {
                DateFrom = startingDateTime,
                DateTo = new DateTime(2021, 2, 1),
                TimeseriesType = "production",
                Granularity = "1 hour"
            };

            SetupPowerOutputValuesMock(
                numberOfPowerPlants,
                expectedAmountOfValuesPerPowerPlant,
                startingDateTime,
                numberOfMinutesBetweenEachValue
            );

            var response = await _powerOutputService.GetTimeseriesAsync(request);

            Assert.NotNull(response);

            foreach (var item in response)
            {
                Assert.Equal(13, item.GeneratedOutputs.Count);
            }
        }

        private void SetupPowerOutputValuesMock(
            int numberOfPowerPlants, 
            int numberOfValues, 
            DateTime powerOutputValuesInitialDate, 
            int numberOfMinutesBetweenEachValue)
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
                    powerOutputValuesInitialDate = powerOutputValuesInitialDate.AddMinutes(numberOfMinutesBetweenEachValue);

                    powerOutputDatabaseResults.Add(
                    new PowerOutputDatabaseResult()
                    {
                        Id = Guid.NewGuid(),
                        SolarPowerPlantId = spp.Id,
                        Name = spp.Name,
                        Date = powerOutputValuesInitialDate,
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
