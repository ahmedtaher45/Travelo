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
    public class AddReviewUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddReviewUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GenericResponse<string>> AddReview(string userId,AddReviewDto dto)
        {
            var existReview = await _unitOfWork.Reviews.GetUserReviewForHotel
                (userId, dto.HotelId);
            if(existReview != null)
            {
                //throw new InvalidOperationException("You have already reviewed this hotel");
                return GenericResponse<string>.FailureResponse("You have already reviewed this hotel");
            }
            var review = new Domain.Models.Entities.Review
            {
                UserId = userId,
                HotelId = dto.HotelId,
                OverallRating = dto.OverallRate,
                AmenityRating = dto.AmenityRate,
                CleanlinessRating = dto.CleanlinessRate,
                CommunicationRating = dto.CommunicationRate,
                LocationRating = dto.LocationRate,
                ValueRating = dto.ValueRate,
                Comment = dto.Comment,
                CreatedOn = DateTime.UtcNow
                

            } ;
            await _unitOfWork.Reviews.Add(review);
            await _unitOfWork.SaveChangesAsync();

            return  GenericResponse<string>.SuccessResponse("Review added successfully");
        }
    }
}
