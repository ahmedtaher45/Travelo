namespace Travelo.API.models
{
    public class BookingPrice
    {
        public int Id { get; set; }
        public int BookingId { get; set; }

        public decimal BaseFare { get; set; }
        public decimal Taxes { get; set; }
        public decimal ServiceFee { get; set; }

        public decimal Total => BaseFare + Taxes + ServiceFee;
    }
}
