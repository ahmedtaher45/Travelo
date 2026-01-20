using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travelo.Application.Common.Responses;
using Travelo.Application.DTOs.Common;
using Travelo.Application.DTOs.Hotels;
using Travelo.Domain.Models.Entities;

namespace Travelo.Application.Interfaces
{
    public interface IHotelRepository
    {
       
        Task<GenericResponse<IEnumerable<HotelCardDto>>> GetFeaturedHotelsAsync(PaginationRequest request);
        Task<GenericResponse<HotelDetailsDto>> GetHotelByIdAsync(int id);



    }
}
