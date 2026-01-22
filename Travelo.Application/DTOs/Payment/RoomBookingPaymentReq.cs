namespace Travelo.Application.DTOs.Payment
{
    public class RoomBookingPaymentReq
    {
        public int HotelId { get; set; }
        public int RoomId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
    }
}
