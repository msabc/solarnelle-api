namespace Solarnelle.Domain.Models.Services.OpenMeteo.Dto
{
    public record OpenMeteoLocationRequestDto
    {
        public required decimal Latitude { get; set; }
        
        public required decimal Longitude { get; set; }
    }
}
