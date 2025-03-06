using Microsoft.Extensions.Options;
using Solarnelle.Application.Services.Forecast;
using Solarnelle.Configuration;

namespace Solarnelle.Api.Jobs
{
    public class SolarRadiationForecastJob(
        IServiceProvider serviceProvider,
        IOptions<SolarnelleSettings> solarnelleSettings,
        ILogger<SolarRadiationForecastJob> logger) : BackgroundService
    {
        private readonly int IntervalInHours = solarnelleSettings.Value.SolarRadiationForecastJobSettings.SolarRadiationForecastJobExecutionIntervalInHours;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation($"{nameof(SolarRadiationForecastJob)} is starting.");

            using PeriodicTimer timer = new(TimeSpan.FromHours(IntervalInHours));

            try
            {
                while (await timer.WaitForNextTickAsync(stoppingToken))
                {
                    await ExecuteSolarRadiationForecastJobAsync();
                }
            }
            catch (OperationCanceledException)
            {
                logger.LogInformation("Timed Hosted Service is stopping.");
            }
        }

        private async Task ExecuteSolarRadiationForecastJobAsync()
        {
            logger.LogInformation($"{nameof(SolarRadiationForecastJob)} interval triggered at {DateTime.UtcNow}.");

            try
            {
                using var scope = serviceProvider.CreateScope();

                var solarRadiationForecastService = scope.ServiceProvider.GetRequiredService<ISolarRadiationForecastService>();

                await solarRadiationForecastService.SaveSolarRadiationForecastsAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error executing {nameof(SolarRadiationForecastJob)}. {ex.Message} {ex.InnerException?.Message}");
            }
        }
    }
}