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

        public HotelsController(GetFeaturedHotelsUseCase getFeaturedHotelsUseCase)
        {
            _getFeaturedHotelsUseCase = getFeaturedHotelsUseCase;
        }

        [HttpGet("featured")]
        // الرابط النهائي: GET /api/hotels/featured
        public async Task<IActionResult> GetFeatured([FromQuery] PaginationRequest request)
        {
            // بنبعت الطلب للـ UseCase
            var result = await _getFeaturedHotelsUseCase.ExecuteAsync(request);

            // بنرد بـ 200 OK ومعاه الداتا
            return Ok(result);
        }

    }
}
