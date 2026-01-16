using Microsoft.EntityFrameworkCore;
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
    public class UpdateReviewUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateReviewUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GenericResponse<string>> UpdateReview(string userId,int reviewId,UpdateReviewDto dto)
        {
            var review = await _unitOfWork.Reviews.GetById(
                reviewId,
                query => query.Include(r=>r.Hotel).Include(r=>r.User)
                );
            if(review == null){
                return  GenericResponse<string>.FailureResponse("Review not found");
            }
            if(review.UserId != userId){
                throw new UnauthorizedAccessException("You can only update your own reviews");
            };
            review.OverallRating = dto.OverallRate;
            review.ValueRating = dto.ValueRate;
            review.LocationRating = dto.LocationRate;
            review.CleanlinessRating = dto.CleanlinessRate;
            review.AmenityRating = dto.AmenityRate;
            review.CommunicationRating = dto.CommunicationRate;
            review.Comment = dto.Comment;
             _unitOfWork.Reviews.Update(review);
            await _unitOfWork.SaveChangesAsync();
            return GenericResponse<string>.SuccessResponse("Review updated successfully");
        }
    }

}
