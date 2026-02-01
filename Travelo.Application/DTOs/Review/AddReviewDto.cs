using System.ComponentModel.DataAnnotations;

namespace Travelo.Application.DTOs.Review
{
    public class AddReviewDto
    {
        public int? HotelId { get; set; }
        public int? FlightId { get; set; }
        public int? AirlineId { get; set; }

        [Required]
        [Range(1, 5)]
        public decimal OverallRate { get; set; }

        [Range(1, 5)] public decimal? AmenityRate { get; set; }
        [Range(1, 5)] public decimal? CleanlinessRate { get; set; }
        [Range(1, 5)] public decimal? CommunicationRate { get; set; }
        [Range(1, 5)] public decimal? LocationRate { get; set; }
        [Range(1, 5)] public decimal? ValueRate { get; set; }

        [MaxLength(2000)]
        public string Comment { get; set; } = null!;
    }

}
