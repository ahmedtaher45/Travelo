using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travelo.Domain.Shared;

namespace Travelo.Domain.Models.Entities
{
    public class Hotel : BaseEntity
    {
        public string? ResponsibleName { get; set; }
        public string? Address { get; set; }
        public string? Country { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int CityId { get; set; }
        public City? City { get; set; }


        // ==========================================

        public string Name { get; set; } = string.Empty; 
        public decimal PricePerNight { get; set; } 
        public double Rating { get; set; } 
        public int ReviewsCount { get; set; } 
        public string ImageUrl { get; set; } = string.Empty; 
        public bool IsFeatured { get; set; }
        public string Description { get; set; } = string.Empty;
        public ICollection<Room> Rooms { get; set; } = new List<Room>();



        public IEnumerable<Review>? Reviews { get; set; } = new List<Review>();
    }
}
