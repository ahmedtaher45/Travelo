using Travelo.Application.Common.Responses;
using Travelo.Application.DTOs.Review;
using Travelo.Application.Interfaces;

namespace Travelo.Application.UseCases.Review
{
    public class AddReviewUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddReviewUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<GenericResponse<string>> AddReview(string userId, AddReviewDto dto)
        {
            var existReview = await _unitOfWork.Reviews.GetUserReviewForEntity(userId, dto.HotelId, dto.FlightId, dto.AirlineId);
            if (existReview.Data != null)
                return GenericResponse<string>.FailureResponse("You already reviewed this entity");

            var review = new Travelo.Domain.Models.Entities.Review
            {
                UserId = userId,
                HotelId = dto.HotelId,
                FlightId = dto.FlightId,
                AirlineId = dto.AirlineId,
                OverallRating = dto.OverallRate,
                AmenityRating = dto.AmenityRate,
                CleanlinessRating = dto.CleanlinessRate,
                CommunicationRating = dto.CommunicationRate,
                LocationRating = dto.LocationRate,
                ValueRating = dto.ValueRate,
                Comment = dto.Comment,
                CreatedOn = DateTime.UtcNow
            };

            await _unitOfWork.Reviews.Add(review);
            await _unitOfWork.SaveChangesAsync();

            return GenericResponse<string>.SuccessResponse("Review added successfully");
        }


    }
}
