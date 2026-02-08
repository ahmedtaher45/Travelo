using Travelo.Application.Common.Responses;
using Travelo.Application.DTOs.Hotels;
using Travelo.Application.DTOs.Room;

namespace Travelo.Application.Services.Room
{
    public interface IRoomService
    {
        Task<GenericResponse<string>> CreateRoom (RoomReqDTO roomReq, string userId, string role);
        Task<GenericResponse<string>> UpdateRoom (int roomId, RoomReqDTO roomReq, string userId, string role);
        Task<GenericResponse<string>> RemoveRoom (int roomId , string userId, string role);
        Task<GenericResponse<RoomDto>> GetRoom (int roomId);
    }
}
