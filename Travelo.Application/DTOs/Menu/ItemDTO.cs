using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travelo.Application.DTOs.Menu
{
    public class ItemDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public List<string>? Images { get; set; }
        public int Calories { get; set; }
        public int? PrepTime { get; set; }
        public int? Stock { get; set; }
        public List<string>? Ingredients { get; set; }
    }



}
