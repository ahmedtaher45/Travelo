using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travelo.Application.DTOs.Hotels
{
     public class HotelDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal PricePerNight { get; set; }
        public double Rating { get; set; }
        public int ReviewsCount { get; set; } 
        public string ImageUrl { get; set; } = string.Empty;       
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public List<string> Gallery { get; set; } = new(); 
        public List<string> Amenities { get; set; } = new();
        public List<RoomDto> Rooms { get; set; } = new();


    }
}
