namespace Travelo.API.models
{
    public class SearchFlight
    {
       public int Id  { get; set; }
        public string FromCity { get; set; }
        public string ToCity { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string TripType { get; set; }
        public int Passengers { get; set; }
        public string ClassType { get; set; }
    }
}
