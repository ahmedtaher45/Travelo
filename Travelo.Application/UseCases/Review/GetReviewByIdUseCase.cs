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
    public class GetReviewByIdUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetReviewByIdUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GenericResponse<ReviewDto>> GetReviewById(int id)
        {
            var review = await _unitOfWork.Reviews.GetById(id);
            return new GenericResponse<ReviewDto>
            {
                Data = new ReviewDto
                {
                    Id = review.Id,
                    UserId = review.UserId,
                    UserName = review.User.UserName,
                    HotelId = review.HotelId,
                    HotelName = review.Hotel.Name,
                    OverallRate = review.OverallRating,
                    CleanlinessRate = review.CleanlinessRating,
                    LocationRate = review.LocationRating,
                    ValueRate = review.ValueRating,
                    AmenityRate = review.AmenityRating,
                    CommunicationRate = review.CommunicationRating,
                    Comment = review.Comment,
                    Status = review.Status,
                    CreatedAt = review.CreatedOn ?? DateTime.UtcNow
                }
            };
        }
    }
}
