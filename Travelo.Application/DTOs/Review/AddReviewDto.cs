using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travelo.Application.DTOs.Review
{
    public class AddReviewDto
    {
        [Required]
        public int HotelId { get; set; }
        [Required]
        [Range(1, 5, ErrorMessage = "Overall Rate must be between 1 and 5")]
        public decimal OverallRate { get; set; }
        [Range(1, 5, ErrorMessage = "Amenity rate must be between 1 and 5")]
        public decimal? AmenityRate { get; set; }
        [Range(1, 5, ErrorMessage = "Cleanliness rate must be between 1 and 5")]
        public decimal? CleanlinessRate { get; set; }
        [Range(1, 5, ErrorMessage = "Communication rate must be between 1 and 5")]
        public decimal? CommunicationRate { get; set; }
        [Range(1, 5, ErrorMessage = "Location rate must be between 1 and 5")]
        public decimal? LocationRate { get; set; }
        [Range(1, 5, ErrorMessage = "Value rate must be between 1 and 5")]
        public decimal? ValueRate { get; set; }
        [MaxLength(2000, ErrorMessage = "Comment cannot exceed 2000 characters")]
        public string Comment { get; set; }
    }
}
