using Travelo.Domain.Models.Enums;
using Travelo.Domain.Shared;

namespace Travelo.Domain.Models.Entities
{
    public class FlightBooking : BaseEntity
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int FlightId { get; set; }
        public Flight Flight { get; set; }
        public DateTime BookingDate { get; set; } = DateTime.Now; public int NumberOfPassengers { get; set; }
        public FlightBookingStatus Status { get; set; }
    }
}
