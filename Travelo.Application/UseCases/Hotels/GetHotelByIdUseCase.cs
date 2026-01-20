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
    public class GetHotelByIdUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetHotelByIdUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<GenericResponse<HotelDetailsDto>> ExecuteAsync(int id)
        {
            return await _unitOfWork.Hotels.GetHotelByIdAsync(id);
        }

    }
}
