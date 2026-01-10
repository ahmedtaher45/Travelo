using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travelo.Domain.Shared;

namespace Travelo.Domain.Models.Entities
{
    public class MenuCategory : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image {  get; set; }

        public IEnumerable<MenuItem> items { get; set; } = new List<MenuItem>();


    }
}
