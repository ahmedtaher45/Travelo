using Diagnosis.Domain.Models.Entites;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
