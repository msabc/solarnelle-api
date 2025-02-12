namespace Solarnelle.Application.Models.Request.PowerOutput
{
    public record GetPowerOutputRequest
    {
        public required string TimeseriesType { get; set; }

        public required string Granularity { get; set; }

        public required DateTime DateFrom { get; set; }

        public required DateTime DateTo { get; set; }
    }
}
