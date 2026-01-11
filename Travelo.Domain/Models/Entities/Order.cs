using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travelo.Domain.Models.Enums;
using Travelo.Domain.Shared;

namespace Travelo.Domain.Models.Entities
{
    public class Order : BaseEntity
    {
        public int UserId { get; set; }

        public OrderStatus Status { get; set; }

        public decimal SubTotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }

        public PaymentType PaymentType { get; set; }

        public IEnumerable<OrderItem> Items { get; set; } = new List<OrderItem>();
       
    }
}
