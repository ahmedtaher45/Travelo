using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travelo.Domain.Models.Enums;
using Travelo.Domain.Shared;

namespace Travelo.Domain.Models.Entities
{
    public class Review : BaseEntity
    {
        [Required]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        [Required]
        public int HotelId { get; set; }
        public virtual Hotel Hotel { get; set; }
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
        public string Comment { get; set; }

        public ReviewStatus Status { get; set; }



    }
}
