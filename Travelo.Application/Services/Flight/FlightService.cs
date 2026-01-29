using Travelo.Application.DTOs.Flight;
using Travelo.Application.Interfaces;

namespace Travelo.Application.Services.Flight
{
    public class FlightService : IFlightService
    {
        private readonly IFlightRepository _flightRepository;

        public FlightService (IFlightRepository flightRepository)
        {
            _flightRepository=flightRepository;
        }

        public async Task<List<FlightDto>> GetAllFlightsAsync ()
        {
            var flights = await _flightRepository.GetAllAsync();
            return flights.Select(f => MapToDto(f)).ToList();
        }

        public async Task<FlightDto?> GetFlightByIdAsync (int id)
        {
            var f = await _flightRepository.GetByIdAsync(id);
            return f==null ? null : MapToDto(f);
        }

        public async Task<FlightDto> CreateFlightAsync (FlightDto flightDto)
        {
            var flight = new Travelo.Domain.Models.Entities.Flight
            {
                Airline=flightDto.Airline,
                FlightNumber=flightDto.FlightNumber,
                FromAirport=flightDto.FromAirport,
                ToAirport=flightDto.ToAirport,
                DepartureDateTime=flightDto.DepartureDateTime,
                ArrivalDateTime=flightDto.ArrivalDateTime,
                Price=flightDto.Price,
                AircraftId=flightDto.AircraftId,
            };

            await _flightRepository.AddAsync(flight);

            flightDto.Id=flight.Id;
            return flightDto;
        }

        public async Task<FlightDto?> UpdateFlightAsync (int id, FlightDto flightDto)
        {
            var flight = await _flightRepository.GetByIdAsync(id);
            if (flight==null) return null;

            flight.Airline=flightDto.Airline;
            flight.FlightNumber=flightDto.FlightNumber;
            flight.FromAirport=flightDto.FromAirport;
            flight.ToAirport=flightDto.ToAirport;
            flight.DepartureDateTime=flightDto.DepartureDateTime;
            flight.ArrivalDateTime=flightDto.ArrivalDateTime;
            flight.Price=flightDto.Price;
            flight.AircraftId=flightDto.AircraftId;

            await _flightRepository.UpdateAsync(flight);

            return MapToDto(flight);
        }

        public async Task<bool> DeleteFlightAsync (int id)
        {
            var flight = await _flightRepository.GetByIdAsync(id);
            if (flight==null) return false;

            await _flightRepository.DeleteAsync(id);
            return true;
        }


        private FlightDto MapToDto (Travelo.Domain.Models.Entities.Flight f)
        {
            return new FlightDto
            {
                Id=f.Id,
                Airline=f.Airline!,
                FlightNumber=f.FlightNumber!,
                FromAirport=f.FromAirport!,
                ToAirport=f.ToAirport!,
                DepartureDateTime=f.DepartureDateTime,
                ArrivalDateTime=f.ArrivalDateTime,
                Price=f.Price,
                AircraftId=f.AircraftId
            };
        }
    }
}
