using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travelo.Domain.Shared;

namespace Travelo.Domain.Models.Entities
{
    public class ThingToDo : BaseEntity
    {
        public string Title { get; set; } = string.Empty;       
        public string Category { get; set; } = string.Empty;    
        public string Distance { get; set; } = string.Empty;   
        public decimal Price { get; set; }                     
        public decimal? OldPrice { get; set; }                 
        public string ImageUrl { get; set; } = string.Empty;

        public int HotelId { get; set; }
        public Hotel? Hotel { get; set; }
    }
}
