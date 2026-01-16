using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Travelo.Application.DTOs.Review;
using Travelo.Application.UseCases.Review;

namespace Travelo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly AddReviewUseCase _addReviewUseCase;
        private readonly GetReviewByIdUseCase _getReviewByIdUseCase;
        private readonly GetUserReviewsUseCase _getUserReviewsUseCase;
        private readonly GetHotelReviewsUseCase _getHotelReviewsUseCase;
        private readonly UpdateReviewUseCase _updateReviewUseCase;
        private readonly RemoveReviewUseCase _removeReviewUseCase;

        public ReviewController(
            AddReviewUseCase addReviewUseCase,
            GetReviewByIdUseCase getReviewByIdUseCase,
            GetUserReviewsUseCase getUserReviewsUseCase,
            GetHotelReviewsUseCase getHotelReviewsUseCase,
            UpdateReviewUseCase updateReviewUseCase,
            RemoveReviewUseCase removeReviewUseCase
            )
        {
            _addReviewUseCase = addReviewUseCase;
            _getReviewByIdUseCase = getReviewByIdUseCase;
            _getUserReviewsUseCase = getUserReviewsUseCase;
            _getHotelReviewsUseCase = getHotelReviewsUseCase;
            _updateReviewUseCase = updateReviewUseCase;
            _removeReviewUseCase = removeReviewUseCase;
        }

        private string GetCurrentUserId() => 
            User.FindFirstValue(ClaimTypes.NameIdentifier)??
            throw new UnauthorizedAccessException("User is not authenticated");

        [HttpPost]
        public async Task<IActionResult> AddReview([FromBody] AddReviewDto dto)
        {
            try
            {
                var userId = GetCurrentUserId();
                var review  = await _addReviewUseCase.AddReview( userId,dto);
                return Created( "", review);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("id")]
        public async Task<IActionResult> UpdateReview([FromRoute] int id, [FromBody] UpdateReviewDto dto)
        {
            try
            {
                var userId = GetCurrentUserId();
                var review = await _updateReviewUseCase.UpdateReview(userId, id, dto);
                return Ok(review);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("id")]
        public async Task<IActionResult> RemoveReview([FromRoute] int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                await _removeReviewUseCase.RemoveReview( id, userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Get: /api/review/id
        [HttpGet("id")]
        [AllowAnonymous]
        public async Task<IActionResult> GetReviewById([FromRoute] int id)
        {
            try
            {
                var review = await _getReviewByIdUseCase.GetReviewById(id)
                    if(review == null)
                {
                    return NotFound();
                }
                return Ok(review);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Get: /api/hotel/reviews/hotelId
        [HttpGet("hotel/{hotelId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetHotelReviews([FromRoute] int hotelId)
        {
            try
            {
                var reviews = await _getHotelReviewsUseCase.GetHotelReview(hotelId);
                return Ok(reviews);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Get: /api/user/reviews
        [HttpGet("my-reviews")]
        public async Task<IActionResult> GetUserReviews()
        {
            try
            {
                var userId = GetCurrentUserId();
                var reviews = await _getUserReviewsUseCase.GetUserReviews(userId);
                return Ok(reviews);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
