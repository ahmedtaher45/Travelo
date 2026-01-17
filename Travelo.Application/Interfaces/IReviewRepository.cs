using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travelo.Application.Common.Responses;
using Travelo.Domain.Models.Entities;
using Travelo.Domain.Models.Enums;

namespace Travelo.Application.Interfaces
{
    public interface IReviewRepository : IGenericRepository<Review>
    {
        Task<GenericResponse<List<Review>>>  GetHotelReviews(
            int hotelId,
            int? pageNum = null,
            int? pageSize = null);
        Task<GenericResponse<List<Review>>> GetUserReviews(
            string userId,
            int? pageNum = null,
            int? pageSize = null);
        Task<GenericResponse<Review>> GetUserReviewForHotel(string userId, int hotelId);
    }
}
