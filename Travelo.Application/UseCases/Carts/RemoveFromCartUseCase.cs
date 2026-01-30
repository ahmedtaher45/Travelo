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
    public class RemoveFromCartUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public RemoveFromCartUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GenericResponse<CartDTO>> ExecuteAsync(
            string userId,
            RemoveFromCartDTO dto)
        {
            var cart = await _unitOfWork.Cart.GetCartByUserId(userId);

            if (cart == null)
                return GenericResponse<CartDTO>.FailureResponse("Cart not found");

            var cartItem = cart.CartItems
                .FirstOrDefault(x => x.ProductId == dto.ProductId);

            if (cartItem == null)
                return GenericResponse<CartDTO>.FailureResponse("Item not found in cart");

            var product = await _unitOfWork.Repository<MenuItem>()
                .GetById(dto.ProductId);

            if (product == null || product.IsDeleted)
                return GenericResponse<CartDTO>.FailureResponse("Product not found");

           
            cartItem.Quantity -= dto.Quantity;

            if (cartItem.Quantity <= 0)
            {
                cart.CartItems.Remove(cartItem);
            }

            
            cart.SubTotal -= dto.Quantity * product.Price;

            if (cart.SubTotal < 0)
                cart.SubTotal = 0;

            cart.TotalPrice = cart.SubTotal;

            await _unitOfWork.SaveChangesAsync();

            return GenericResponse<CartDTO>.SuccessResponse(MapCart(cart));
        }

        private CartDTO MapCart(Cart cart)
        {
            return new CartDTO
            {
                Id = cart.Id,
                SubTotal = cart.SubTotal,
                TotalPrice = cart.TotalPrice,
                Items = cart.CartItems.Select(i => new CartItemDTO
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    ProductName = i.Product!.Name,
                    Price = i.Product.Price,
                    Quantity = i.Quantity,
                    Total = i.Quantity * i.Product.Price
                }).ToList()
            };
        }
    }
 }
