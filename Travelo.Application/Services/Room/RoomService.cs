using Travelo.Application.Common.Responses;
using Travelo.Application.DTOs.Hotels;
using Travelo.Application.DTOs.Room;
using Travelo.Application.Interfaces;
using Travelo.Domain.Models.Entities;

namespace Travelo.Application.Services.Room
{
    public class RoomService : IRoomService
    {
        private readonly IUnitOfWork unitOfWork;

        public RoomService (IUnitOfWork unitOfWork)
        {
            this.unitOfWork=unitOfWork;
        }

        private async Task<bool> IsAuthorizedToEditAsync(int hotelId, string userId, string role)
        {          
            if (role == "Admin") return true;
     
            var hotel = await unitOfWork.Hotels.GetById(hotelId);
            if (hotel == null) return false;
            return hotel.UserId == userId;
        }
        public async Task<GenericResponse<string>> CreateRoom (RoomReqDTO roomReq, string userId, string role)
        {

            if (!await IsAuthorizedToEditAsync(roomReq.HotelId, userId, role))
            {
                return GenericResponse<string>.FailureResponse("Unauthorized: You do not own this hotel.");
            }
            var isExsisHotel = await unitOfWork.Hotels.GetById(roomReq.HotelId);
            if (isExsisHotel==null)
            {
                return GenericResponse<string>.FailureResponse("InValed Hotel Id ");
            }
            var room = new Travelo.Domain.Models.Entities.Room
            {
                Type=roomReq.Type,
                Capacity=roomReq.Capacity,
                PricePerNight=roomReq.PricePerNight,
                View=roomReq.View,
                ImageUrl=roomReq.ImageUrl,
                HotelId=roomReq.HotelId,
                BedType=roomReq.BedType,
                Size=roomReq.Size
            };
            await unitOfWork.Rooms.Add(room);
            await unitOfWork.SaveChangesAsync();
            return GenericResponse<string>.SuccessResponse("room added successfully");
        }

        public async Task<GenericResponse<RoomDto>> GetRoom (int roomId)
        {
            var data = await unitOfWork.Rooms.GetById(roomId);
            if (data==null) return GenericResponse<RoomDto>.FailureResponse("Room not found ");
            var dtoData = new RoomDto
            {
                Id=data.Id,
                Price=data.PricePerNight,
                Type=data.Type,
                Capacity=data.Capacity,
                View=data.View,
                ImageUrl=data.ImageUrl,
                BedType=data.BedType,
                Size=data.Size,
                RoomAmenities=new List<string> { "Breakfast", "Free Wifi", "AC" },
                IsAvailable=data.IsAvailable
            };
            return GenericResponse<RoomDto>.SuccessResponse(dtoData);
        }

        public async Task<GenericResponse<string>> RemoveRoom (int roomId, string userId, string role)
        {
            var room = await unitOfWork.Rooms.GetById(roomId);       
            if (room == null) return GenericResponse<string>.FailureResponse("Room not found");
          
            if (!await IsAuthorizedToEditAsync(room.HotelId, userId, role))
            {
                return GenericResponse<string>.FailureResponse("Unauthorized: You do not own this room.");
            }

            unitOfWork.Rooms.Delete(room);
            await unitOfWork.SaveChangesAsync();
            return GenericResponse<string>.SuccessResponse("Room deleted successfully.");
        }

        public async Task<GenericResponse<string>> UpdateRoom (int roomId, RoomReqDTO roomReq , string userId, string role)
        {
            var room = await unitOfWork.Rooms.GetById(roomId);
            if (room == null) return GenericResponse<string>.FailureResponse("Room not found");
            if (!await IsAuthorizedToEditAsync(room.HotelId, userId, role))
            {
                return GenericResponse<string>.FailureResponse("Unauthorized: You do not own this room.");
            }

            room.Type = roomReq.Type;
            room.PricePerNight = roomReq.PricePerNight;
            room.Capacity = roomReq.Capacity;
            room.View = roomReq.View;
            room.ImageUrl = roomReq.ImageUrl;
            room.IsAvailable = roomReq.IsAvailable;
            room.BedType = roomReq.BedType;
            room.Size = roomReq.Size;

            unitOfWork.Rooms.Update(room);
            await unitOfWork.SaveChangesAsync();
            return GenericResponse<string>.SuccessResponse("Room updated successfully.");
        }
    }
}
