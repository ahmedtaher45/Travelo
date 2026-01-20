using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travelo.Application.DTOs.Common;
using Travelo.Application.Interfaces;
using Travelo.Application.DTOs.Hotels;
using Travelo.Application.Common.Responses;

namespace Travelo.Application.UseCases.Hotels
{
    public class GetFeaturedHotelsUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetFeaturedHotelsUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GenericResponse<IEnumerable<HotelCardDto>>> ExecuteAsync(PaginationRequest request)
        {       
            return await _unitOfWork.Hotels.GetFeaturedHotelsAsync(request);
        }

    }
}