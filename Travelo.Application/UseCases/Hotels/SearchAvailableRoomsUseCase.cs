using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travelo.Application.Common.Responses;
using Travelo.Application.DTOs.Hotels;
using Travelo.Application.Interfaces;
using Travelo.Domain.Models.Entities;

namespace Travelo.Application.UseCases.Hotels
{
    public class SearchAvailableRoomsUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public SearchAvailableRoomsUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GenericResponse<List<RoomDto>>> ExecuteAsync(RoomSearchDto dto)
        {
            // 1️⃣ Validate Dates
            if (dto.CheckOut <= dto.CheckIn)
            {
                return GenericResponse<List<RoomDto>>
                    .FailureResponse("Check-out date must be after check-in date.");
            }

            // 2️⃣ Check Hotel Exists
            var hotel = await _unitOfWork
                .Repository<Hotel>()
                .GetById(dto.HotelId);

            if (hotel == null)
            {
                return GenericResponse<List<RoomDto>>
                    .FailureResponse("Hotel not found.");
            }


            var rooms = await _unitOfWork.Rooms
                            .GetAvailableRoomsAsync(dto.HotelId, dto.CheckIn, dto.CheckOut);

            if (!rooms.Any())
                return GenericResponse<List<RoomDto>>
                    .FailureResponse("No available rooms");

            var roomDtos = rooms.Select(r => new RoomDto
            {
                Id = r.Id,
                Type = r.Type,
                Price = r.PricePerNight,
                Capacity = r.Capacity,
                View = r.View,
                ImageUrl = r.ImageUrl,
                BedType = r.BedType,
                Size = r.Size,
                IsAvailable = true,
                RoomAmenities = new List<string> { "Breakfast", "Free Wifi", "AC" }
            }).ToList();

            return GenericResponse<List<RoomDto>>
                .SuccessResponse(roomDtos);
        }
    }
}
