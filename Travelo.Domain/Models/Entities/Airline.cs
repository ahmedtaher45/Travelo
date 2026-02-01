using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travelo.Domain.Shared;

namespace Travelo.Domain.Models.Entities
{
    public class Airline : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string LogoUrl { get; set; } = null!;
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
