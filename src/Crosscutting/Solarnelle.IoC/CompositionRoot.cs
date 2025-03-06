using System.Threading;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Solarnelle.Application.Services.AccessToken;
using Solarnelle.Application.Services.Forecast;
using Solarnelle.Application.Services.PowerOutput;
using Solarnelle.Application.Services.SolarPowerPlant;
using Solarnelle.Application.Services.User;
using Solarnelle.Application.Services.Validation.PowerOutput;
using Solarnelle.Application.Services.Validation.SolarPowerPlant;
using Solarnelle.Configuration;
using Solarnelle.Domain.Interfaces.DatabaseContext;
using Solarnelle.Domain.Interfaces.Repositories;
using Solarnelle.Domain.Interfaces.Services;
using Solarnelle.Domain.Models.Tables;
using Solarnelle.Infrastructure.DatabaseContext;
using Solarnelle.Infrastructure.DataSeeding;
using Solarnelle.Infrastructure.Repositories;
using Solarnelle.Infrastructure.Services.OpenMeteo;

namespace Solarnelle.IoC
{
    public static class CompositionRoot
    {
        public static SolarnelleSettings RegisterApplicationDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = services.RegisterSettings(configuration);

            services.RegisterDatabaseConfiguration(settings)
                    .RegisterDbContext()
                    .RegisterRepositories()
                    .RegisterHttpClients(settings)
                    .RegisterInfrastructureServices()
                    .RegisterApplicationServices();

            return settings;
        }

        private static SolarnelleSettings RegisterSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SolarnelleSettings>(options => configuration.GetSection(nameof(SolarnelleSettings)).Bind(options));

            var solarnelleSettings = new SolarnelleSettings();
            configuration.GetSection(nameof(SolarnelleSettings)).Bind(solarnelleSettings);

            return solarnelleSettings;
        }

        private static IServiceCollection RegisterDatabaseConfiguration(this IServiceCollection services, SolarnelleSettings settings)
        {
            services.AddDbContext<SolarnelleDbContext>(options =>
            {
                options.UseSqlServer(settings.DatabaseSettings.SolarnelleConnectionString)
                       .UseSeeding((context, _) =>
                       {
                           SolarnelleDatabaseSeeder.SeedDatabaseAsync(context).Wait();
                       })
                       .UseAsyncSeeding(async (context, _, cancellationToken) =>
                       {
                           await SolarnelleDatabaseSeeder.SeedDatabaseAsync(context);
                       });
            });

            return services;
        }

        private static IServiceCollection RegisterDbContext(this IServiceCollection services)
        {
            services.AddScoped<ISolarnelleDbContext, SolarnelleDbContext>();

            return services;
        }

        private static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<ISolarPowerPlantRepository, SolarPowerPlantRepository>();
            services.AddScoped<IForecastedValuesRepository, ForecastedValuesRepository>();
            services.AddScoped<IProductionValuesRepository, ProductionValuesRepository>();
            services.AddScoped<ISolarRadiationForecastRepository, SolarRadiationForecastRepository>();

            return services;
        }

        private static IServiceCollection RegisterHttpClients(this IServiceCollection services, SolarnelleSettings solarnelleSettings)
        {
            services.AddHttpClient<OpenMeteoWeatherForecastService>(client =>
            {
                client.BaseAddress = new Uri(solarnelleSettings.OpenMeteoAPISettings.BaseURL);
            });

            return services;
        }

        private static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services)
        {
            services.AddTransient<IOpenMeteoWeatherForecastService, OpenMeteoWeatherForecastService>();

            return services;
        }

        private static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAccessTokenService, AccessTokenService>();

            services.AddScoped<ISolarPowerPlantService, SolarPowerPlantService>();
            services.AddScoped<IPowerOutputService, PowerOutputService>();
            services.AddScoped<ISolarRadiationForecastService, SolarRadiationForecastService>();


            // validation
            services.AddScoped<ISolarPowerPlantValidationService, SolarPowerPlantValidationService>();
            services.AddScoped<IPowerOutputValidationService, PowerOutputValidationService>();

            return services;
        }
    }
}
