using Travelo.Application.Common.Responses;
using Travelo.Application.DTOs.Hotels;
using Travelo.Application.DTOs.Room;

namespace Travelo.Application.Services.Room
{
    public interface IRoomService
    {
        Task<GenericResponse<string>> CreateRoom (RoomReqDTO roomReq);
        Task<GenericResponse<string>> UpdateRoom (int roomId, RoomReqDTO roomReq);
        Task<GenericResponse<string>> RemoveRoom (int roomId);
        Task<GenericResponse<RoomDto>> GetRoom (int roomId);
    }
}
