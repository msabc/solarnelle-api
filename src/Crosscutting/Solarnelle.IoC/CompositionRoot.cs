using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Solarnelle.Application.Services.AccessToken;
using Solarnelle.Application.Services.PowerOutput;
using Solarnelle.Application.Services.SolarPowerPlant;
using Solarnelle.Application.Services.User;
using Solarnelle.Application.Services.Validation.PowerOutput;
using Solarnelle.Application.Services.Validation.SolarPowerPlant;
using Solarnelle.Configuration;
using Solarnelle.Domain.Interfaces.DatabaseContext;
using Solarnelle.Domain.Interfaces.Repositories;
using Solarnelle.Infrastructure.DatabaseContext;
using Solarnelle.Infrastructure.Repositories;

namespace Solarnelle.IoC
{
    public static class CompositionRoot
    {
        public static IServiceCollection RegisterApplicationDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterSettings(configuration)
                    .RegisterDatabaseConfiguration()
                    .RegisterDbContext()
                    .RegisterRepositories()
                    .RegisterApplicationServices();

            return services;
        }

        private static IServiceCollection RegisterSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DatabaseSettings>(options => configuration.GetSection(nameof(DatabaseSettings)).Bind(options));
            services.Configure<SolarnelleSettings>(options => configuration.GetSection(nameof(SolarnelleSettings)).Bind(options));

            return services;
        }

        private static IServiceCollection RegisterDatabaseConfiguration(this IServiceCollection services)
        {
            ServiceProvider serviceProvider = services.BuildServiceProvider();

            using IServiceScope scope = serviceProvider.CreateScope();

            var databaseSettings = scope.ServiceProvider.GetRequiredService<IOptions<DatabaseSettings>>().Value;

            services.AddDbContext<SolarnelleDbContext>(options =>
            {
                options.UseSqlServer(databaseSettings.SolarnelleConnectionString);
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
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ISolarPowerPlantRepository, SolarPowerPlantRepository>();
            services.AddScoped<IForecastedValuesRepository, ForecastedValuesRepository>();
            services.AddScoped<IProductionValuesRepository, ProductionValuesRepository>();

            return services;
        }

        private static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAccessTokenService, AccessTokenService>();

            services.AddScoped<ISolarPowerPlantService, SolarPowerPlantService>();
            services.AddScoped<IPowerOutputService, PowerOutputService>();

            // validation
            services.AddScoped<ISolarPowerPlantValidationService, SolarPowerPlantValidationService>();
            services.AddScoped<IPowerOutputValidationService, PowerOutputValidationService>();

            return services;
        }
    }
}
