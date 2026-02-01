using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travelo.Domain.Models.Enums;

namespace Travelo.Application.DTOs.Review
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserImage { get; set; }

        public int? HotelId { get; set; }
        public string? HotelName { get; set; }

        public int? FlightId { get; set; }
        public string? FlightNumber { get; set; }

        public int? AirlineId { get; set; }
        public string? AirlineName { get; set; }

        public decimal OverallRate { get; set; }
        public decimal? AmenityRate { get; set; }
        public decimal? CleanlinessRate { get; set; }
        public decimal? CommunicationRate { get; set; }
        public decimal? LocationRate { get; set; }
        public decimal? ValueRate { get; set; }
        public string Comment { get; set; }
        public ReviewStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
