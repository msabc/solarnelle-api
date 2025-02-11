using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Solarnelle.Application.Services.Auth;
using Solarnelle.Application.Services.SolarPowerPlant;
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

            return services;
        }

        private static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ISolarPowerPlantService, SolarPowerPlantService>();

            return services;
        }
    }
}
