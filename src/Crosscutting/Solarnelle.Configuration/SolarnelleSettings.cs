using Solarnelle.Configuration.Models.Auth;
using Solarnelle.Configuration.Models.Database;
using Solarnelle.Configuration.Models.External;

namespace Solarnelle.Configuration
{
    public class SolarnelleSettings
    {
        public const string ApplicationName = "Solarnelle.Api";

        public DatabaseSettingsElement DatabaseSettings {  get; set; }

        public JWTSettingsElement JWTSettings { get; set; }

        public OpenMeteoAPISettingsElement OpenMeteoAPISettings { get; set; }

        public int ForecastBackgroundJobExecutionIntervalInMinutes { get; set; }
    }
}
