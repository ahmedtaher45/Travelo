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
    public class GetAirlineReviewsUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAirlineReviewsUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GenericResponse<List<AirlineReviewDto>>> GetAirlineReview(
            int airlineId,
            int? pageNum = 0,
            int? pageSize = 3)
        {
            var reviews = await _unitOfWork.Reviews.GetAirlineReviews(
                airlineId, pageNum, pageSize);

            var reviewDto = reviews.Data.Select(
                v => new AirlineReviewDto
                {
                    AirlineId = airlineId,
                    AvgOverallRate = reviews.Data.Average(r => r.OverallRating),
                    AvgValueRate = reviews.Data.Average(r => r.ValueRating) ?? 0,
                    AvgLocationRate = reviews.Data.Average(r => r.LocationRating) ?? 0,
                    AvgCleanlinessRate = reviews.Data.Average(r => r.CleanlinessRating) ?? 0,
                    AvgAmenityRate = reviews.Data.Average(r => r.AmenityRating) ?? 0,
                    AvgCommunicationRate = reviews.Data.Average(r => r.CommunicationRating) ?? 0,
                    Reviews = reviews.Data.Select(
                        r => new ReviewDto
                        {
                            Id = r.Id,
                            UserId = r.UserId,
                            UserName = r.User.UserName,
                            AirlineId = airlineId,
                            OverallRate = r.OverallRating,
                            CleanlinessRate = r.CleanlinessRating,
                            LocationRate = r.LocationRating,
                            ValueRate = r.ValueRating,
                            AmenityRate = r.AmenityRating,
                            CommunicationRate = r.CommunicationRating,
                            Comment = r.Comment,
                            Status = r.Status,
                            CreatedAt = r.CreatedOn ?? DateTime.UtcNow
                        }

                        ).ToList()

                }
                );
            return new GenericResponse<List<AirlineReviewDto>> { Data = reviewDto.ToList() };

        }
    }
}