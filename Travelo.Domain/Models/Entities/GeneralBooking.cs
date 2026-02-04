using Travelo.Domain.Models.Enums;
using Travelo.Domain.Shared;

namespace Travelo.Domain.Models.Entities
{
    public class GeneralBooking : BaseEntity
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public BookingType Type { get; set; }

        public int? FlightId { get; set; }
        public Flight? Flight { get; set; }

        public int? HotelId { get; set; }
        public Hotel? Hotel { get; set; }

        public int? RoomId { get; set; }
        public Room? Room { get; set; }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public BookingStatus Status { get; set; }
        public decimal TotalPrice { get; set; }
        public BookingPrice? PriceDetails { get; set; }
    }

}
