using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travelo.Application.Common.Responses;
using Travelo.Application.DTOs.Cart;
using Travelo.Application.Interfaces;
using Travelo.Domain.Models.Entities;

namespace Travelo.Application.UseCases.Carts
{
    public class GetCartUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCartUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GenericResponse<CartDTO>> ExecuteAsync(string userId)
        {
            var userCart = await _unitOfWork.Cart
                .GetCartByUserId(userId);
            if (userCart == null)
            {
                return GenericResponse<CartDTO>.FailureResponse("Cart not found");
            }


            var dto = new CartDTO
            {
                Id = userCart.Id,
                Items = userCart.CartItems.Select(ci => new CartItemDTO
                {
                    Id = ci.Id,
                    ProductId = ci.ProductId,
                    ProductName = ci.Product!.Name,
                    Price = ci.Product.Price,
                    Quantity = ci.Quantity,
                    Total = ci.Quantity * ci.Product.Price
                }).ToList()
            };

            dto.SubTotal = dto.Items.Sum(i => i.Total);
            dto.TotalPrice = dto.SubTotal;

            return GenericResponse<CartDTO>.SuccessResponse(dto);
        }
    }

}
