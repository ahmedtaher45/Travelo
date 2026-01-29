using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travelo.Application.DTOs.SupportTicket
{
    public class SupportTicketReplyDTO
    {
        public string userId {  get; set; }
        public int TicketId { get; set; }
        public string Subject { get; set; }
        public string Details { get; set; }
        public string Reply {  get; set; }
    }
}
