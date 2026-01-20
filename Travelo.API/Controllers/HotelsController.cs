using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Travelo.Application.DTOs.Common;
using Travelo.Application.UseCases.Hotels;

namespace Travelo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {

        private readonly GetFeaturedHotelsUseCase _getFeaturedHotelsUseCase;
        private readonly GetHotelByIdUseCase _getHotelByIdUseCase;
        public HotelsController(
            GetFeaturedHotelsUseCase getFeaturedHotelsUseCase,
            GetHotelByIdUseCase getHotelByIdUseCase)
        {
            _getFeaturedHotelsUseCase = getFeaturedHotelsUseCase;
            _getHotelByIdUseCase = getHotelByIdUseCase;
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
    }
}
