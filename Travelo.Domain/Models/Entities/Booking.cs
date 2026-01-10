using Travelo.Domain.Shared;

namespace Travelo.Domain.models.Entities
{
    public class Booking : BaseEntity
    {
        public int FlightId { get; set; }
        public Flight? Flight { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
