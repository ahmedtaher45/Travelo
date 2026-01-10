using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travelo.Domain.Shared;

namespace Travelo.Domain.Models.Entities
{
    public class Cart : BaseEntity
    {
        public int UserId { get; set; }

        public decimal TotalPrice { get; set; }

        public decimal SubTotal { get; set; }
        public List<CartItems> CartItems { get; set; }
    }
}
