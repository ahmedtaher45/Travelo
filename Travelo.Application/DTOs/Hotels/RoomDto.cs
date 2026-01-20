using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travelo.Application.DTOs.Hotels
{
    public class RoomDto
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public decimal Price { get; set; }
        public int Capacity { get; set; }
        public string View { get; set; }
        public string ImageUrl { get; set; }

        public string BedType { get; set; }
        public int Size { get; set; }
        public List<string> RoomAmenities { get; set; } = new(); 

        public bool IsAvailable { get; set; }

    }
}
