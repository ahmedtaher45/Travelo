using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travelo.Domain.Shared;

namespace Travelo.Domain.Models.Entities
{
    public class MenuItem : BaseEntity
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public IEnumerable<string> Images { get; set; }

        public int Calories { get; set; }
        public int PrepTime { get; set; }
        public int Stock { get; set; }

        public IEnumerable<string> Ingredients { get; set; }


    }
}
