using Travelo.Domain.Shared;

namespace Travelo.Domain.Models.Entities
{
    public class RoomBooking : BaseEntity
    {
        public int RoomId { get; set; }
        public Room Room { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
    }
}
