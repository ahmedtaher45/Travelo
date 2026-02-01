using System.ComponentModel.DataAnnotations;
using Travelo.Domain.Models.Enums;
using Travelo.Domain.Shared;

namespace Travelo.Domain.Models.Entities
{
    public class Review : BaseEntity
    {
        [Required]
        public string UserId { get; set; } = null!;
        public virtual ApplicationUser User { get; set; } = null!;

        public int? HotelId { get; set; }
        public virtual Hotel? Hotel { get; set; }

        public int? FlightId { get; set; }
        public virtual Flight? Flight { get; set; }

        public int? AirlineId { get; set; }
        public virtual Airline? Airline { get; set; }

        [Range(1, 5)]
        public decimal OverallRating { get; set; }

        [Range(1, 5)]
        public decimal? AmenityRating { get; set; }
        [Range(1, 5)]
        public decimal? CleanlinessRating { get; set; }
        [Range(1, 5)]
        public decimal? CommunicationRating { get; set; }
        [Range(1, 5)]
        public decimal? LocationRating { get; set; }
        [Range(1, 5)]
        public decimal? ValueRating { get; set; }

        [MaxLength(2000)]
        public string Comment { get; set; } = null!;

        public ReviewStatus Status { get; set; } = ReviewStatus.Pending;
    }
}
