using System.ComponentModel;

namespace Solarnelle.Application.Models.Dto.PowerOutput
{
    public record PowerOutputResponseItemDto
    {
        [Description("Date of output. When the type of timeseries is real production, this refers to the measured date. When the type of timeseries is forecasted production, this refers to the date of forecast.")]
        public DateTime Date { get; set; }

        [Description("Power output in kilowatts (kW).")]
        public decimal PowerOutput { get; set; }
    }
}
