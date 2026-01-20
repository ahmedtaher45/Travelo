using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travelo.Application.DTOs.Hotels
{
    public class ThingToDoDto
    {
        public int Id { get; set; }
        public string Title { get; set; }      
        public string Category { get; set; }    
        public string Distance { get; set; }   
        public decimal Price { get; set; }      
        public decimal? OldPrice { get; set; }  
        public string ImageUrl { get; set; }    
    }
}
