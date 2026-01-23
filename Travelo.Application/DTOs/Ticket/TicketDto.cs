using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travelo.Domain.Models.Enums;

namespace Travelo.Application.DTOs.Ticket
{
    public class TicketDto
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public string TicketNumber { get; set; } = null!;
        public string SeatNumber { get; set; } = null!;
        public string Gate { get; set; } = null!;
        public string Barcode { get; set; } = null!;
        public TicketStatus Status { get; set; }
    }
}
