using Diagnosis.Domain.Models.Entites;
using System;
using System.Collections.Generic;               
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travelo.Application.DTOs.SupportTicket
{
    public class GetSupportTicketDTO
    {
       
        public string? userName { get; set; }
        public int? Experience { get; set; }
        public string? Subject { get; set; }
        public string? Status { get; set; }
    }
}
