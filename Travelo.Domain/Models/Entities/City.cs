using Travelo.Domain.Shared;

namespace Travelo.Domain.Models.Entities
{
    public class City : BaseEntity
    {
        public string? Name { get; set; }
        public string? Country { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }

        public IEnumerable<Hotel>? Hotels { get; set; }
        public string ImgUrl { get; set; }
        // public  IEnumerable<Restorant>
         public IEnumerable<Restaurant>? Restaurants { get; set; } = new List<Restaurant>();
    }
}
