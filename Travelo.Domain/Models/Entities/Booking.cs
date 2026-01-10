namespace Travelo.Domain.models.Entities
{
    public class Booking
    {
        public int Id { get; set; }
        public int FlightId { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
