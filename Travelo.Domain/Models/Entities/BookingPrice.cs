using Travelo.Domain.Shared;

namespace Travelo.Domain.models.Entities
{
    public class BookingPrice : BaseEntity
    {
        public int BookingId { get; set; }
        public Booking? Booking { get; set; }
        public decimal BaseFare { get; set; }
        public decimal Taxes { get; set; }
        public decimal ServiceFee { get; set; }

        public decimal Total => BaseFare + Taxes + ServiceFee;
    }
}
