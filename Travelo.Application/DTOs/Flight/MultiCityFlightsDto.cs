using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travelo.Application.DTOs.Flight
{
    public class MultiCityFlightsDto
    {
        public List<List<FlightDto>> SegmentsFlights { get; set; } = new();
    }
}

