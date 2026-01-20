using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travelo.Application.DTOs.Hotels
{
    public class HotelCardDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public double Rating { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public int ReviewsCount { get; set; }

    }
}