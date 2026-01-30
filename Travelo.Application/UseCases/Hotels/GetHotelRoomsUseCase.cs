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
    public class GetHotelRoomsUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetHotelRoomsUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GenericResponse<IEnumerable<RoomDto>>> ExecuteAsync(int hotelId)
        {
            return await _unitOfWork.Hotels.GetRoomsByHotelIdAsync(hotelId);
        }
    }
}
