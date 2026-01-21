using Microsoft.AspNetCore.Identity;

namespace Travelo.Domain.Models.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public bool PasswordRestCode { get; set; }

        public IEnumerable<Review>? Reviews { get; set; } = new List<Review>();
    }

}
