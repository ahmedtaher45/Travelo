using Travelo.Domain.Shared;

namespace Travelo.Domain.Models.Entities
{
    public class Flight : BaseEntity
    {
        public string? Airline { get; set; }
        public string? FlightNumber { get; set; }

        public string? FromAirport { get; set; }
        public string? ToAirport { get; set; }

        public DateTime DepartureDateTime { get; set; }
        public DateTime ArrivalDateTime { get; set; }

        public TimeSpan Duration { get; set; }
        public bool Stop { get; set; }

        public decimal Price { get; set; }

        public int AircraftId { get; set; }
    }
}
