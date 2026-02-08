using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travelo.Application.Common.Responses;
using Travelo.Application.DTOs.Hotels;
using Travelo.Application.Interfaces;

namespace Travelo.Application.UseCases.Hotels
{
    public class UpdateHotelUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateHotelUseCase(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<GenericResponse<string>> ExecuteAsync(int id, UpdateHotelDto dto, string currentUserId, bool isAdmin)
        {
            return await _unitOfWork.Hotels.UpdateHotelAsync(id, dto, currentUserId, isAdmin);
        }
    }
}
