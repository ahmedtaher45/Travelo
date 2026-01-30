using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travelo.Application.DTOs.Ticket
{
    public class CreateTicketDto
    {
        public int BookingId { get; set; }
        public string SeatNumber { get; set; } = null!;
        public string Gate { get; set; } = null!;
    }
}
