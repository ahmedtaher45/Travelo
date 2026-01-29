using Travelo.Domain.Models.Enums;
using Travelo.Domain.Shared;

namespace Travelo.Domain.Models.Entities
{
    public class Booking : BaseEntity
    {
        public int FlightId { get; set; }
        public Flight? Flight { get; set; }
        public BookingStatus Status { get; set; }
        public decimal TotalPrice { get; set; }
        public Ticket? Ticket { get; set; }
    }
}
