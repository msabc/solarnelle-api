using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Solarnelle.Configuration;
using Solarnelle.Infrastructure.DatabaseContext;

namespace Solarnelle.IoC
{
    public static class CompositionRoot
    {
        public static IServiceCollection RegisterApplicationDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterSettings(configuration)
                    .RegisterDbContext();

            return services;
        }

        private static IServiceCollection RegisterSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DatabaseSettings>(options => configuration.GetSection(nameof(DatabaseSettings)).Bind(options));

            return services;
        }

        private static IServiceCollection RegisterDbContext(this IServiceCollection services)
        {
            ServiceProvider serviceProvider = services.BuildServiceProvider();

            using IServiceScope scope = serviceProvider.CreateScope();

            var databaseOptions = scope.ServiceProvider.GetService<IOptions<DatabaseSettings>>();

            if (databaseOptions is null)
                throw new Exception($"Unable to start the application due to missing {nameof(DatabaseSettings)}.");
            
            var databaseSettings = databaseOptions.Value;

            services.AddDbContext<SolarnelleDbContext>(options =>
            {
                options.UseSqlServer(databaseSettings.SolarnelleConnectionString);
            });

            return services;
        }
    }
}
