using System.ComponentModel;

namespace Solarnelle.Application.Models.Request.PowerOutput
{
    public record GetPowerOutputRequest
    {
        [Description("Granularity of the response. Possible values:'production' or 'forecast'. Default is 'production'.")]
        public required string TimeseriesType { get; set; }

        [Description("Granularity of the response. Possible value:'15 min' or '1 hour'. Default is '15 min'")]
        public required string Granularity { get; set; }

        public required DateTime DateFrom { get; set; }

        public required DateTime DateTo { get; set; }
    }
}
