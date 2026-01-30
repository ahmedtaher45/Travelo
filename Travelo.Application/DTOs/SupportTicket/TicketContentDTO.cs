using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travelo.Application.DTOs.SupportTicket
{
    public class TicketContentDTO
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public string? Subject { get; set; }
        public string? Details { get; set; }
    }
}
