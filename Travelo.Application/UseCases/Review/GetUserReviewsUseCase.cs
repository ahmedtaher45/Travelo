using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travelo.Application.Common.Responses;
using Travelo.Application.DTOs.Review;
using Travelo.Application.Interfaces;

namespace Travelo.Application.UseCases.Review
{
    public class GetUserReviewsUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUserReviewsUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GenericResponse<List<ReviewDto>>> GetUserReviews(
            string userId, int? pageNum = 0, int? pageSize = 10)
        {
            var reviews = await _unitOfWork.Reviews.GetUserReviews(
                userId, pageNum, pageSize);

            var reviewDtos = reviews.Data.Select(
                r => new ReviewDto
                {
                    Id = r.Id,
                    UserId = r.UserId,
                    UserName = r.User.UserName,
                    HotelId = r.HotelId,
                    HotelName = r.Hotel.Name,
                    OverallRate = r.OverallRating,
                    CleanlinessRate = r.CleanlinessRating,
                    LocationRate = r.LocationRating,
                    ValueRate = r.ValueRating,
                    AmenityRate = r.AmenityRating,
                    CommunicationRate = r.CommunicationRating,
                   Comment = r.Comment,
                   Status = r.Status,
                   CreatedAt = r.CreatedOn ?? DateTime.UtcNow
                }).ToList();

            return new GenericResponse<List<ReviewDto>> { Data = reviewDtos };
                
        }
    }
}
