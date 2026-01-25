using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travelo.Application.Common.Responses;
using Travelo.Application.Interfaces;
using Travelo.Domain.Models.Entities;

namespace Travelo.Application.UseCases.Carts
{
    public class RemoveCartItemUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public RemoveCartItemUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GenericResponse<string>> ExecuteAsync(int cartItemId)
        {
            var cartItem = await _unitOfWork.Repository<CartItem>()
                .GetById(cartItemId);

            if (cartItem == null)
            {
                return GenericResponse<string>.FailureResponse("Item not found");
            }

            _unitOfWork.Repository<CartItem>().Delete(cartItem);
            await _unitOfWork.SaveChangesAsync();

            return GenericResponse<string>.SuccessResponse("Item removed from cart");
        }
    }

}
