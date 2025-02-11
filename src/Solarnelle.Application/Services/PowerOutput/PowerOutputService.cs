using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Solarnelle.Application.Models.Enums;
using Solarnelle.Application.Models.Request.PowerOutput;
using Solarnelle.Application.Models.Response.PowerOutput;
using Solarnelle.Domain.Interfaces.Repositories;
using Solarnelle.Domain.Models.DatabaseResults;

namespace Solarnelle.Application.Services.PowerOutput
{
    public class PowerOutputService(
        IProductionValuesRepository productionValuesRepository,
        IForecastedValuesRepository forecastedValuesRepository,
        ILogger<PowerOutputService> logger) : IPowerOutputService
    {
        public async Task<IEnumerable<GetPowerOutputResponse>> GetTimeseriesAsync(GetPowerOutputRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
