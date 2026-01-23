using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travelo.Domain.Shared;

namespace Travelo.Domain.Models.Entities
{
    public class Restaurant:BaseEntity
    {
        public int CityId { get; set; }
        public City City { get; set; }
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<MenuCategory> MenuCategories { get; set; }

    }
}
