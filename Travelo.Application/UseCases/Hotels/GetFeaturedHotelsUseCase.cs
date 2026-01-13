using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travelo.Application.DTOs.Common;
using Travelo.Application.Interfaces;
using Travelo.Application.DTOs.Hotels;

namespace Travelo.Application.UseCases.Hotels
{
    public class GetFeaturedHotelsUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetFeaturedHotelsUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<HotelCardDto>> ExecuteAsync(PaginationRequest request)
        {
            
            var hotels = await _unitOfWork.Hotels.GetFeaturedHotelsAsync(request);

           
            var hotelDtos = hotels.Select(h => new HotelCardDto
            {
                Id = h.Id,
                Name = h.Name,
             
                Location = $"{h.City?.Name ?? h.Address}, {h.Country}",

                Price = h.PricePerNight,

                Rating = h.Rating,
                ImageUrl = h.ImageUrl
            });

            return hotelDtos;
        }

    }
}
