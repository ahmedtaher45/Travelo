using System.ComponentModel.DataAnnotations;

namespace Travelo.Application.DTOs.SupportTicket
{
    public class SupportTicketDTO
    {


        [MaxLength(150)]
        public string? Subject { get; set; }

        [MaxLength(2000)]
        public string? Details { get; set; }
    }

}
