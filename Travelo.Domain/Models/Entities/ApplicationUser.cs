using Microsoft.AspNetCore.Identity;

namespace Travelo.Domain.Models.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public Hotel? Hotel { get; set; }
        public Restaurant? Restaurant { get; set; }
        public bool PasswordRestCode { get; set; }

        public IEnumerable<Review>? Reviews { get; set; } = new List<Review>();
    }

}
