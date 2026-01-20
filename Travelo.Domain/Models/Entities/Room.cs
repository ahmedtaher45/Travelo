using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travelo.Domain.Shared;

namespace Travelo.Domain.Models.Entities
{
    public class Room : BaseEntity
    {
        public string Type { get; set; } = string.Empty; 
        public decimal PricePerNight { get; set; } 
        public int Capacity { get; set; } 
        public string View { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsAvailable { get; set; } = true;

       
        public int HotelId { get; set; }
        public Hotel? Hotel { get; set; }
        public string BedType { get; set; } = string.Empty; 
        public int Size { get; set; } 
    }
}
