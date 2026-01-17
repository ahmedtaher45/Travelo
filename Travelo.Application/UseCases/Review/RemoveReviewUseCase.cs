using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travelo.Application.Common.Responses;
using Travelo.Application.Interfaces;

namespace Travelo.Application.UseCases.Review
{
    public class RemoveReviewUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        public RemoveReviewUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GenericResponse<string>> RemoveReview(int id, string userId){

            var review  = await _unitOfWork.Reviews.GetById(id);

            if (review == null) { 
                return  GenericResponse<string>.FailureResponse("Review not found");
            }
            if (review.UserId != userId) { 
                throw new UnauthorizedAccessException("You are not authorized to delete this review.");
            }
            _unitOfWork.Reviews.Delete(review);
            await _unitOfWork.SaveChangesAsync();
            return GenericResponse<string>.SuccessResponse("Review deleted successfully");

        }
    }
}
