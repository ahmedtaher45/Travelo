using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travelo.Domain.Models.Enums;

namespace Travelo.Application.DTOs.Flight
{
    public class FlightFilterDto
    {
        public string? From { get; set; }
        public string? To { get; set; }

        public DateTime? DepartureDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        public TripType TripType { get; set; }

        public bool IsFlexibleDates { get; set; }
        public int? FlexDays { get; set; }

        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        public int? AirlineId { get; set; }
        public bool? IsNonStop { get; set; }

        public FlightClass? Class { get; set; }
        public decimal? MinRating { get; set; }

        public List<MultiCitySegmentDto>? Segments { get; set; }
    }
}
