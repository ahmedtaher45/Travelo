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
    public class AddToCartUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddToCartUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GenericResponse<CartDTO>> ExecuteAsync(
            string userId,
            AddToCartDTO dto)
        {
            var cart = await _unitOfWork.Cart
                .GetCartByUserId(userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    CartItems = new List<CartItem>()
                };
                await _unitOfWork.Repository<Cart>().Add(cart);
            }

            var product = await _unitOfWork.Repository<MenuItem>()
                .GetById(dto.ProductId);

            if (product == null || product.IsDeleted)
                return GenericResponse<CartDTO>.FailureResponse("Product not found");

            var cartItem = cart.CartItems
                .FirstOrDefault(x => x.ProductId == dto.ProductId);

            if (cartItem != null)
            {
                cartItem.Quantity += dto.Quantity;
            }
            else
            {
                cart.CartItems.Add(new CartItem
                {
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity
                });
            }

            cart.SubTotal +=dto.Quantity * product.Price;
            
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
