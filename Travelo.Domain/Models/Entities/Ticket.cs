using Travelo.Domain.Models.Enums;
using Travelo.Domain.Shared;

namespace Travelo.Domain.Models.Entities
{
    public class Ticket : BaseEntity
    {
        public int BookingId { get; set; }
        public Booking? Booking { get; set; }
        public string TicketNumber { get; set; } = null!;
        public string SeatNumber { get; set; } = null!;
        public string Gate { get; set; } = null!;
        public string Barcode { get; set; } = null!;
        public FlightClass FlightClass { get; set; }
        public TicketStatus Status { get; set; }
    }

}
