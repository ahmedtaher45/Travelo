using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travelo.Application.DTOs.Flight
{
    public class FlightDto
    {
        public int Id { get; set; }
        public string Airline { get; set; } = null!;
        public string FlightNumber { get; set; } = null!;
        public string FromAirport { get; set; } = null!;
        public string ToAirport { get; set; } = null!;
        public DateTime DepartureDateTime { get; set; }
        public DateTime ArrivalDateTime { get; set; }
        public decimal Price { get; set; }
    }

}
