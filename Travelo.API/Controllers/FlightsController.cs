using Microsoft.AspNetCore.Mvc;
using Travelo.Application.DTOs.Flight;
using Travelo.Application.Services.Flight;

namespace Travelo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private readonly IFlightService _flightService;

        public FlightsController (IFlightService flightService)
        {
            _flightService=flightService;
        }

        [HttpGet]
        public async Task<IActionResult> GetFlights ()
        {
            var flights = await _flightService.GetAllFlightsAsync();
            return Ok(flights);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFlight (int id)
        {
            var flight = await _flightService.GetFlightByIdAsync(id);
            return flight==null ? NotFound() : Ok(flight);
        }


        [HttpPost]
        public async Task<IActionResult> CreateFlight ([FromBody] FlightDto flightDto)
        {
            var flight = await _flightService.CreateFlightAsync(flightDto);
            return CreatedAtAction(nameof(GetFlight), new { id = flight.Id }, flight);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFlight (int id, [FromBody] FlightDto flightDto)
        {
            var updatedFlight = await _flightService.UpdateFlightAsync(id, flightDto);
            return updatedFlight==null ? NotFound() : Ok(updatedFlight);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFlight (int id)
        {
            var deleted = await _flightService.DeleteFlightAsync(id);
            return !deleted ? NotFound() : NoContent();
        }

    }

}
