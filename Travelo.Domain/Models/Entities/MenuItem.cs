using Travelo.Domain.Shared;

namespace Travelo.Domain.Models.Entities
{
    public class MenuItem : BaseEntity
    {
        public int CategoryId { get; set; }
        public MenuCategory? Category { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public List<string>? Images { get; set; }

        public int Calories { get; set; }
        public int PrepTime { get; set; }
        public int Stock { get; set; }

        public List<string>? Ingredients { get; set; }


    }
}
