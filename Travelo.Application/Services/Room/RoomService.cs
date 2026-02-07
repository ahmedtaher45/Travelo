using Travelo.Application.Common.Responses;
using Travelo.Application.DTOs.Hotels;
using Travelo.Application.DTOs.Room;
using Travelo.Application.Interfaces;

namespace Travelo.Application.Services.Room
{
    public class RoomService : IRoomService
    {
        private readonly IUnitOfWork unitOfWork;

        public RoomService (IUnitOfWork unitOfWork)
        {
            this.unitOfWork=unitOfWork;
        }
        public async Task<GenericResponse<string>> CreateRoom (RoomReqDTO roomReq)
        {
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

        public async Task<GenericResponse<string>> RemoveRoom (int roomId)
        {
            var isExsist = await unitOfWork.Rooms.GetById(roomId);
            if (isExsist==null)
            {
                return GenericResponse<string>.FailureResponse("Room not found ");
            }
            unitOfWork.Rooms.Delete(isExsist);
            await unitOfWork.SaveChangesAsync();
            return GenericResponse<string>.FailureResponse("Room deleted successfully.");
        }

        public async Task<GenericResponse<string>> UpdateRoom (int roomId, RoomReqDTO roomReq)
        {
            var isExsist = await unitOfWork.Rooms.GetById(roomId);
            if (isExsist==null)
            {
                return GenericResponse<string>.FailureResponse("Room not found ");
            }
            var update = new Travelo.Domain.Models.Entities.Room
            {
                Id=roomId,
                Type=roomReq.Type,
                PricePerNight=roomReq.PricePerNight,
                Capacity=roomReq.Capacity,
                View=roomReq.View,
                ImageUrl=roomReq.ImageUrl,
                IsAvailable=roomReq.IsAvailable,
                BedType=roomReq.BedType,
                HotelId=isExsist.HotelId,
                Size=roomReq.Size
            };
            unitOfWork.Rooms.Update(update);
            await unitOfWork.SaveChangesAsync();
            return GenericResponse<string>.FailureResponse("Room updated successfully.");
        }
    }
}
