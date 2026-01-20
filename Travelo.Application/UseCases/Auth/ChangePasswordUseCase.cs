using System.Threading.Tasks;
using Travelo.Application.Common.Responses;
using Travelo.Application.DTOs.Auth;
using Travelo.Application.Interfaces;

namespace Travelo.Application.UseCases.Auth
{
    public class ChangePasswordUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        public ChangePasswordUseCase (IUnitOfWork unitOfWork)
        {
            _unitOfWork=unitOfWork;
        }
        public async Task<GenericResponse<string>> ExecuteAsync (ChangePasswordDTO changePasswordDTO, string userId)
        {
            return await _unitOfWork.Auth.ChangePasswordAsync(changePasswordDTO, userId);
        }
    }
}
