using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travelo.Application.DTOs.Cart
{
    public class CartDTO
    {
        public int Id { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TotalPrice { get; set; }
        public List<CartItemDTO> Items { get; set; } = new();
    }

}
