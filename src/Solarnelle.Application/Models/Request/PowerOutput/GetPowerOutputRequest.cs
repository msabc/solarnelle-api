using Solarnelle.Application.Models.Enums;

namespace Solarnelle.Application.Models.Request.PowerOutput
{
    public record GetPowerOutputRequest
    {
        public TimeseriesType Type { get; set; }

        public TimeseriesGranularity Granularity { get; set; }

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }
    }
}
