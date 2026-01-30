
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travelo.Domain.Models.Entities;
using Travelo.Domain.Shared;

namespace Travelo.Domain.Models.Entites
{
    public class SupportTicket: BaseEntity
    {
        public string? userId { get; set; }
        public ApplicationUser? User { get; set; }
        public  string? Subject { get; set; }
        public string? Details { get; set; }
        public string? Status { get; set; }
        public string? Reply { get; set; }
    }
}
