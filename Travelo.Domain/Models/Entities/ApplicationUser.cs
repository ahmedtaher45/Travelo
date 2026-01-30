using Microsoft.AspNetCore.Identity;
using Travelo.Domain.Models.Entites;

namespace Travelo.Domain.Models.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public Hotel? Hotel { get; set; }
        public Restaurant? Restaurant { get; set; }
        // public bool PasswordRestCode { get; set; } = false;

        public IEnumerable<Review>? Reviews { get; set; } = new List<Review>(); 
        public ICollection<SupportTicket>? SupportTickets { get; set; }

    }

}
