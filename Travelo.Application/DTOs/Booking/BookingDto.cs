using Travelo.Domain.Models.Enums;

namespace Travelo.Application.DTOs.Booking
{
    public class BookingDto
    {
        public int Id { get; set; }
        public int FlightId { get; set; }

        public BookingStatus Status { get; set; }
        public decimal TotalPrice { get; set; }

        public int? TicketId { get; set; }
    }

}
