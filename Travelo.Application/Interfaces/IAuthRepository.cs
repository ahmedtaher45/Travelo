using Travelo.Application.Common.Responses;
using Travelo.Application.DTOs.Auth;

namespace Travelo.Application.Interfaces
{
    public interface IAuthRepository
    {
        Task<GenericResponse<string>> RegisterAsync (RegisterDTO registerDTO);
        Task<GenericResponse<string>> ChangePasswordAsync (ChangePasswordDTO changePasswordDTO, string userId);
    }
}
