using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travelo.Application.Common.Responses;
using Travelo.Application.DTOs.Hotels;
using Travelo.Application.DTOs.Restaurant;
using Travelo.Application.Interfaces;

namespace Travelo.Application.UseCases.Hotels
{
    public class AddHotelUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        public AddHotelUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GenericResponse<string>> ExecuteAsync(AddHotelDTO dto)
        {
            var city = await _unitOfWork.Repository<Domain.Models.Entities.City>().GetById(dto.CityId);
            if (city == null)
            {
                return new GenericResponse<string> { Data = "City not found" };
            }

            return await _unitOfWork.Auth.AddHotel(dto.CityId, dto);
        }
    }
}
