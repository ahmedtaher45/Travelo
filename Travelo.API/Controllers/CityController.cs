using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Travelo.Application.DTOs.City;
using Travelo.Application.Services.City;

namespace Travelo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly ICityService cityService;

        public CityController (ICityService cityService)
        {
            this.cityService=cityService;
        }
        [HttpGet]
        public async Task<IActionResult> Get ([FromQuery] int? pagenum = null, [FromQuery] int? pageSize = null)
        {
            var url = $"{Request.Scheme}://{Request.Host}/Cities/";
            var result = await cityService.GetAllCitiesAsync(pagenum, pageSize, url);
            return !result.Success ? BadRequest(result) : Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCityById (int id)
        {
            var url = $"{Request.Scheme}://{Request.Host}/Cities/";
            var result = await cityService.GetCityByIdAsync(id, url);
            return !result.Success ? BadRequest(result) : Ok(result);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCity ([FromForm] CityReqDTO cityReq)
        {
            var result = await cityService.CreateCity(cityReq);
            return !result.Success ? BadRequest(result) : Ok(result);
        }
        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCity (int id, [FromForm] CityReqDTO cityReq)
        {
            var result = await cityService.UpdateCity(id, cityReq);
            return !result.Success ? BadRequest(result) : Ok(result);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCity (int id)
        {
            var result = await cityService.RemoveCity(id);
            return !result.Success ? BadRequest(result) : Ok(result);
        }
        [HttpGet("{id}/restaurants")]
        public async Task<IActionResult> GetCityRestaurants (int id)
        {
            var result = await cityService.GetCityRestorants(id);
            return !result.Success ? BadRequest(result) : Ok(result);
        }
        [HttpGet("{id}/Hotels")]
        public async Task<IActionResult> GetCityHotels (int id)
        {
            var result = await cityService.GetCityHotels(id);
            return !result.Success ? BadRequest(result) : Ok(result);
        }
    }
}
