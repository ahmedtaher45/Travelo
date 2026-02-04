namespace Travelo.Application.DTOs.Payment
{
    public class RoomBookingPaymentRes
    {
        public int id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int HotelId { get; set; }
        public string HotelName { get; set; }
        public int RoomId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
    }
}
