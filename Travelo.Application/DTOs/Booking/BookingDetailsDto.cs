using Travelo.Domain.Models.Enums;

namespace Travelo.Application.DTOs.Booking
{
    public class BookingDetailsDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserNamw { get; set; }
        public string Name { get; set; }

        public string Address { get; set; }

        public BookingType Type { get; set; }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public decimal BasePrice { get; set; }
        public decimal Taxes { get; set; }

        public decimal Total { get; set; }

        public BookingStatus Status { get; set; }
    }

}
