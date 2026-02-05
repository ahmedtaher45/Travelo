namespace Travelo.Application.DTOs.Room
{
    public class RoomReqDTO
    {
        public string Type { get; set; } = string.Empty;
        public decimal PricePerNight { get; set; }
        public int Capacity { get; set; }
        public string View { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsAvailable { get; set; } = true;
        public int HotelId { get; set; }
        public string BedType { get; set; } = string.Empty;
        public int Size { get; set; }
    }
}
