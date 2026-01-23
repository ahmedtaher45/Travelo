using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travelo.Application.Common.Responses;
using Travelo.Application.DTOs.Auth;
using Travelo.Application.DTOs.Restaurant;
using Travelo.Application.Interfaces;

namespace Travelo.Application.UseCases.Auth
{
    public class AddAdminUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        public AddAdminUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GenericResponse<string>> ExecuteAsync(AdminDTO dto)
        {

            return await _unitOfWork.Auth.AddAdmin(dto);
        }
    }
}
