using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travelo.Domain.Models.Entities;

namespace Travelo.Application.DTOs.Menu
{
    public class CategoryDTO
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }

        public List<ItemDTO> Items { get; set; } = new List<ItemDTO>();
    }
}
