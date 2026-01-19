using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travelo.Application.Common.Responses;
using Travelo.Application.DTOs.Auth;
using Travelo.Application.Interfaces;

namespace Travelo.Application.UseCases.Auth
{
    public class ResetPasswordUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ResetPasswordUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<GenericResponse<string>> ExecuteAsync(ResetPasswordDTO resetPasswordDTO)
        {
            return await _unitOfWork.Auth.ResetPasswordAsync(resetPasswordDTO);
        }

    }
}
