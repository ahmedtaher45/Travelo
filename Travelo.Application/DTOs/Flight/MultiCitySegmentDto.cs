using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travelo.Application.DTOs.Flight
{
    public class MultiCitySegmentDto
    {
        public string From { get; set; } = null!;
        public string To { get; set; } = null!;
        public DateTime Date { get; set; }
    }
}

