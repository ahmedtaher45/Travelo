using Travelo.Domain.Models.Enums;
using Travelo.Domain.Shared;

namespace Travelo.Domain.Models.Entities
{
    public class Flight : BaseEntity
    {
        public int AirlineId { get; set; }
        public virtual Airline Airline { get; set; } = null!;

        public string FlightNumber { get; set; } = null!;
        public string FromAirport { get; set; } = null!;
        public string ToAirport { get; set; } = null!;

        public DateTime DepartureDateTime { get; set; }
        public DateTime ArrivalDateTime { get; set; }

        public decimal Price { get; set; }
        public int AvailableSeats { get; set; }
        public FlightClass Class { get; set; } 
        public bool IsNonStop { get; set; }
        public string BaggageAllowance { get; set; } = null!;

        public int AircraftId { get; set; }
        public virtual Aircraft Aircraft { get; set; } = null!;

        public decimal AverageRating { get; set; }
        public int ReviewsCount { get; set; }
    }
}
