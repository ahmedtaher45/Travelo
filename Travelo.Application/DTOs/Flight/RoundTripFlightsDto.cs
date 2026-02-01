using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travelo.Application.DTOs.Flight
{
    public class RoundTripFlightsDto
    {
        public List<FlightDto> OutboundFlights { get; set; } = new();
        public List<FlightDto> ReturnFlights { get; set; } = new();
    }
}