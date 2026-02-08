using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travelo.Application.DTOs.Hotels
{
    public class UpdateHotelDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal PricePerNight { get; set; }
        public string Address { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsFeatured { get; set; }

    }
}
