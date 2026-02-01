using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travelo.Application.DTOs.Flight;

namespace Travelo.Application.Services.Flight
{
    public interface IFlightService
    {
        Task<object> SearchFlightsAsync(FlightFilterDto filter);
        Task<List<FlightDto>> GetAllFlightsAsync();
        Task<FlightDto?> GetFlightByIdAsync(int id);
        Task<FlightDto> CreateFlightAsync(FlightDto flightDto);
        Task<FlightDto?> UpdateFlightAsync(int id, FlightDto flightDto);
        Task<bool> DeleteFlightAsync(int id);
    }

}
