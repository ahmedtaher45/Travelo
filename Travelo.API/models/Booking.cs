namespace Travelo.API.models
{
    public class Booking
    {
        public int Id { get; set; }
        public int FlightId { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
