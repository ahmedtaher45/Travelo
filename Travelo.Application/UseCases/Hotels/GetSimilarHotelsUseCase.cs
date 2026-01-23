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
    public class GetSimilarHotelsUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetSimilarHotelsUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GenericResponse<IEnumerable<HotelCardDto>>> ExecuteAsync(int hotelId)
        {
            return await _unitOfWork.Hotels.GetSimilarHotelsAsync(hotelId);
        }
    }
}
