using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travelo.Application.DTOs.Review
{
    public class HotelReviewDto
    {
        public int HotelId { get; set; }
        public decimal AvgOverallRate { get; set; }
        public decimal AvgCleanlinessRate { get; set; }
        public decimal AvgLocationRate { get; set; }
        public decimal AvgValueRate { get; set; }
        public decimal AvgCommunicationRate { get; set; }
        public decimal AvgAmenityRate { get; set; }
        public List<ReviewDto> Reviews { get; set; } = new List<ReviewDto>();

    }
}
