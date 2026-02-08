using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travelo.Application.Common.Responses;
using Travelo.Application.Interfaces;

namespace Travelo.Application.UseCases.Hotels
{
    public class DeleteHotelUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteHotelUseCase(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<GenericResponse<string>> ExecuteAsync(int id)
        {
            return await _unitOfWork.Hotels.DeleteHotelAsync(id);
        }
    }
}
