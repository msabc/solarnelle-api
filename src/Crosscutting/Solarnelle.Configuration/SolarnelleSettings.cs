using Solarnelle.Configuration.Models;

namespace Solarnelle.Configuration
{
    public class SolarnelleSettings
    {
        public const string ApplicationName = "Solarnelle.Api";

        public required JWTSettingsElement JWTSettings { get; set; }
    }
}
