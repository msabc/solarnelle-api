using Solarnelle.Application.Models.Request.PowerOutput;
using Solarnelle.Domain.Enums;

namespace Solarnelle.Application.Services.Validation.PowerOutput
{
    public interface IPowerOutputValidationService
    {
        (TimeseriesType, TimeseriesGranularity) ValidateGetTimeseriesRequest(GetPowerOutputRequest request);
    }
}
