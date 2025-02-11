using Solarnelle.Application.Models.Request.PowerOutput;
using Solarnelle.Application.Models.Response.PowerOutput;

namespace Solarnelle.Application.Services.PowerOutput
{
    public interface IPowerOutputService
    {
        Task<IEnumerable<GetPowerOutputResponse>> GetTimeseriesAsync(GetPowerOutputRequest request);
    }
}
