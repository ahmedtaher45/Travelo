using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travelo.Application.DTOs.Menu
{
    public class AddItemDTO
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public List<IFormFile>? Images { get; set; }

        public int Calories { get; set; }
        public int PrepTime { get; set; }
        public int Stock { get; set; }

        public List<string>? Ingredients { get; set; }
    }
}
