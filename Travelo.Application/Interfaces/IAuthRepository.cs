using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travelo.Application.Common.Responses;
using Travelo.Application.DTOs.Auth;

namespace Travelo.Application.Interfaces
{
    public interface IAuthRepository
    {
        Task<GenericResponse<string>> RegisterAsync(RegisterDTO registerDTO);
        Task<GenericResponse<AuthResponseDTO>> LoginAsync(LoginDTO loginDTO);
    }
}
