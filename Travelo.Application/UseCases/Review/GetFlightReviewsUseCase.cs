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
    public class GetFlightReviewsUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetFlightReviewsUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GenericResponse<List<FlightReviewDto>>> GetFlightReviews(
            int flightId,
            int? pageNum = 0,
            int? pageSize = 3)
        {
            var reviews = await _unitOfWork.Reviews.GetFlightReviews(
                flightId, pageNum, pageSize);

            var reviewDto = reviews.Data.Select(
                v => new FlightReviewDto
                {
                    FlightId = flightId,
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
                            FlightId = flightId,
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
            return new GenericResponse<List<FlightReviewDto>> { Data = reviewDto.ToList() };


        }
    }
}
