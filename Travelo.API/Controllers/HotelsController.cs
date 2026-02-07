using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Travelo.Application.Common.Responses;
using Travelo.Application.DTOs.Common;
using Travelo.Application.DTOs.Hotels;
using Travelo.Application.DTOs.Review;
using Travelo.Application.UseCases.Hotels;
using Travelo.Application.UseCases.Review;

namespace Travelo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {

        private readonly GetFeaturedHotelsUseCase _getFeaturedHotelsUseCase;
        private readonly GetHotelByIdUseCase _getHotelByIdUseCase;
        private readonly GetHotelRoomsUseCase _getHotelRoomsUseCase;
        private readonly GetHotelReviewsUseCase _getHotelReviewsUseCase;
        private readonly GetThingsToDoUseCase _getThingsToDoUseCase;
        private readonly GetSimilarHotelsUseCase _getSimilarHotelsUseCase;
        private readonly SearchAvailableRoomsUseCase _searchAvailableRoomsUse;
        private readonly GetAllHotelsUseCase _getAllHotelsUseCase;

        public HotelsController(
            GetFeaturedHotelsUseCase getFeaturedHotelsUseCase,
            GetHotelByIdUseCase getHotelByIdUseCase,
            GetHotelRoomsUseCase getHotelRoomsUseCase,
            GetHotelReviewsUseCase getHotelReviewsUseCase,
            GetThingsToDoUseCase getThingsToDoUseCase,
            GetSimilarHotelsUseCase getSimilarHotelsUseCase,
            SearchAvailableRoomsUseCase searchAvailableRoomsUse,
            GetAllHotelsUseCase getAllHotelsUseCase)
        {
            _getFeaturedHotelsUseCase = getFeaturedHotelsUseCase;
            _getHotelByIdUseCase = getHotelByIdUseCase;
            _getHotelRoomsUseCase = getHotelRoomsUseCase;
            _getHotelReviewsUseCase = getHotelReviewsUseCase;
            _getThingsToDoUseCase = getThingsToDoUseCase;
            _getSimilarHotelsUseCase = getSimilarHotelsUseCase;
            _searchAvailableRoomsUse = searchAvailableRoomsUse;
            _getAllHotelsUseCase = getAllHotelsUseCase;
        }


        [HttpGet("featured")]
        public async Task<IActionResult> GetFeatured([FromQuery] PaginationRequest request)
        {
            var response = await _getFeaturedHotelsUseCase.ExecuteAsync(request);
            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllHotels([FromQuery] PaginationRequest request)
        {
            var response = await _getAllHotelsUseCase.ExecuteAsync(request);

            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _getHotelByIdUseCase.ExecuteAsync(id);
            if (response.Success)
            {
                return Ok(response);
            }

            if (response.Message == "Hotel not found")
            {
                return NotFound(response);
            }
            return BadRequest(response);
        }



        [HttpGet("{hotelId}/rooms")]
        public async Task<IActionResult> GetRooms(int hotelId)
        {
            var response = await _getHotelRoomsUseCase.ExecuteAsync(hotelId);

            if (response.Success)
                return Ok(response);

            return BadRequest(response);
        }

       
       
        [HttpGet("{hotelId}/reviews")]
        public async Task<IActionResult> GetHotelReviews(
            int hotelId,
            [FromQuery] int pageNum = 1,
            [FromQuery] int pageSize = 5)
        {
            var response = await _getHotelReviewsUseCase.GetHotelReview(hotelId, pageNum, pageSize);

            if (response.Success)
                return Ok(response);

            return BadRequest(response);
        }


        
        [HttpGet("{hotelId}/things-to-do")]
        public async Task<IActionResult> GetThingsToDo(int hotelId)
        {
            var response = await _getThingsToDoUseCase.ExecuteAsync(hotelId);

            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }


        [HttpGet("{hotelId}/similar")]
        public async Task<IActionResult> GetSimilarHotels(int hotelId)
        {
            var response = await _getSimilarHotelsUseCase.ExecuteAsync(hotelId);

            if (response.Success)
                return Ok(response);

            return BadRequest(response);
        }

        [HttpPost("search")]
        public async Task<IActionResult> SearchAvailableRooms(
            [FromBody] RoomSearchDto dto)
        {
            var result = await _searchAvailableRoomsUse.ExecuteAsync(dto);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

    }
}
