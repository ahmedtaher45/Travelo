using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travelo.Application.Common.Responses;
using Travelo.Application.DTOs.Common;
using Travelo.Application.DTOs.Hotels;
using Travelo.Application.Interfaces;

namespace Travelo.Application.UseCases.Hotels
{
    public class GetAllHotelsUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllHotelsUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GenericResponse<IEnumerable<HotelCardDto>>> ExecuteAsync(PaginationRequest request)
        {
            return await _unitOfWork.Hotels.GetAllHotelsAsync(request);
        }
    }
}
