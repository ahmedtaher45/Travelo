using Microsoft.EntityFrameworkCore;
using Travelo.Application.DTOs.Flight;
using Travelo.Application.Interfaces;
using Travelo.Domain.Models.Enums;
using FlightEntity = Travelo.Domain.Models.Entities.Flight;

namespace Travelo.Application.Services.Flight
{
    public class FlightService : IFlightService
    {
        private readonly IFlightRepository _flightRepository;

        public FlightService(IFlightRepository flightRepository)
        {
            _flightRepository = flightRepository;
        }

        public async Task<object> SearchFlightsAsync(FlightFilterDto filter)
        {
            return filter.TripType switch
            {
                TripType.OneWay => await SearchOneWayAsync(filter),
                TripType.RoundTrip => await SearchRoundTripAsync(filter),
                TripType.MultiCity => await SearchMultiCityAsync(filter),
                _ => throw new Exception("Invalid trip type")
            };
        }

        private async Task<List<FlightDto>> SearchOneWayAsync(FlightFilterDto filter)
        {
            if (!filter.DepartureDate.HasValue)
                throw new Exception("Departure date is required");

            var query = ApplyCommonFilters(
                _flightRepository.GetAllQueryable(), filter);

            query = query
                .Where(f => f.FromAirport == filter.From)
                .Where(f => f.ToAirport == filter.To);

            query = ApplyDateFilter(query, filter.DepartureDate.Value, filter);

            var flights = await query.ToListAsync();
            return flights.Select(f => MapToDto(f)).ToList();
        }

        private async Task<RoundTripFlightsDto> SearchRoundTripAsync(FlightFilterDto filter)
        {
            if (!filter.DepartureDate.HasValue || !filter.ReturnDate.HasValue)
                throw new Exception("Departure & return dates are required");

            var outbound = ApplyCommonFilters(
                _flightRepository.GetAllQueryable(), filter)
                .Where(f => f.FromAirport == filter.From)
                .Where(f => f.ToAirport == filter.To);

            outbound = ApplyDateFilter(outbound, filter.DepartureDate.Value, filter);

            var @return = ApplyCommonFilters(
                _flightRepository.GetAllQueryable(), filter)
                .Where(f => f.FromAirport == filter.To)
                .Where(f => f.ToAirport == filter.From);

            @return = ApplyDateFilter(@return, filter.ReturnDate.Value, filter);

            return new RoundTripFlightsDto
            {
                OutboundFlights = (await outbound.ToListAsync())
                                    .Select(f => MapToDto(f)).ToList(),

                ReturnFlights = (await @return.ToListAsync())
                                    .Select(f => MapToDto(f)).ToList()
            };
        }

        private async Task<MultiCityFlightsDto> SearchMultiCityAsync(FlightFilterDto filter)
        {
            if (filter.Segments == null || !filter.Segments.Any())
                throw new Exception("MultiCity segments are required");

            var result = new MultiCityFlightsDto();

            foreach (var segment in filter.Segments)
            {
                var query = ApplyCommonFilters(
                    _flightRepository.GetAllQueryable(), filter)
                    .Where(f => f.FromAirport == segment.From)
                    .Where(f => f.ToAirport == segment.To)
                    //.Where(f =>
                    //f.DepartureDateTime >= segment.Date.Date &&
                    //f.DepartureDateTime < segment.Date.Date.AddDays(1))

                    .Where(f => f.DepartureDateTime.Date == segment.Date.Date);

                var flights = await query.ToListAsync();
                result.SegmentsFlights.Add(
                    flights.Select(f => MapToDto(f)).ToList()
                );
            }

            return result;
        }

        private IQueryable<FlightEntity> ApplyCommonFilters(
            IQueryable<FlightEntity> query,
            FlightFilterDto filter)
        {
            if (filter.MinPrice.HasValue)
                query = query.Where(f => f.Price >= filter.MinPrice);

            if (filter.MaxPrice.HasValue)
                query = query.Where(f => f.Price <= filter.MaxPrice);

            if (filter.AirlineId.HasValue)
                query = query.Where(f => f.AirlineId == filter.AirlineId);

            if (filter.IsNonStop.HasValue)
                query = query.Where(f => f.IsNonStop == filter.IsNonStop);

            if (filter.Class.HasValue)
                query = query.Where(f => f.Class == filter.Class);

            if (filter.MinRating.HasValue)
                query = query.Where(f => f.AverageRating >= filter.MinRating);

            return query;
        }

        private IQueryable<FlightEntity> ApplyDateFilter(
            IQueryable<FlightEntity> query,
            DateTime date,
            FlightFilterDto filter)
        {
            if (filter.IsFlexibleDates && filter.FlexDays.HasValue)
            {
                var from = date.AddDays(-filter.FlexDays.Value);
                var to = date.AddDays(filter.FlexDays.Value);

                return query.Where(f =>
                    f.DepartureDateTime.Date >= from.Date &&
                    f.DepartureDateTime.Date <= to.Date);
            }

            return query.Where(f => f.DepartureDateTime.Date == date.Date);
        }

        public async Task<List<FlightDto>> GetAllFlightsAsync()
        {
            var flights = await _flightRepository.GetAllQueryable().ToListAsync();
            return flights.Select(f => MapToDto(f)).ToList();
        }

        public async Task<FlightDto?> GetFlightByIdAsync(int id)
        {
            var flight = await _flightRepository.GetByIdAsync(id);
            return flight == null ? null : MapToDto(flight);
        }

        public async Task<FlightDto> CreateFlightAsync(FlightDto dto)
        {
            var flight = new FlightEntity
            {
                AirlineId = dto.AirlineId,
                AircraftId = dto.AircraftId,
                FlightNumber = dto.FlightNumber,
                FromAirport = dto.FromAirport,
                ToAirport = dto.ToAirport,
                DepartureDateTime = dto.DepartureDateTime,
                ArrivalDateTime = dto.ArrivalDateTime,
                Price = dto.Price,
                BaggageAllowance = dto.Baggage,
                IsNonStop = dto.NonStop
            };

            await _flightRepository.AddAsync(flight);
            dto.Id = flight.Id;
            return dto;
        }

        public async Task<FlightDto?> UpdateFlightAsync(int id, FlightDto dto)
        {
            var flight = await _flightRepository.GetByIdAsync(id);
            if (flight == null) return null;

            flight.AirlineId = dto.AirlineId;
            flight.AircraftId = dto.AircraftId;
            flight.FlightNumber = dto.FlightNumber;
            flight.FromAirport = dto.FromAirport;
            flight.ToAirport = dto.ToAirport;
            flight.DepartureDateTime = dto.DepartureDateTime;
            flight.ArrivalDateTime = dto.ArrivalDateTime;
            flight.Price = dto.Price;
            flight.BaggageAllowance = dto.Baggage;
            flight.IsNonStop = dto.NonStop;
            flight.AverageRating = dto.Rating;

            await _flightRepository.UpdateAsync(flight);
            return MapToDto(flight);
        }

        public async Task<bool> DeleteFlightAsync(int id)
        {
            var flight = await _flightRepository.GetByIdAsync(id);
            if (flight == null) return false;

            await _flightRepository.DeleteAsync(id);
            return true;
        }

        // ================= MAPPER =================
        private FlightDto MapToDto(FlightEntity f)
        {
            return new FlightDto
            {
                Id = f.Id,
                AirlineId = f.AirlineId,
                AircraftId = f.AircraftId,
                AirlineName = f.Airline?.Name ?? "N/A",
                AirlineLogo = f.Airline?.LogoUrl ?? "",
                FlightNumber = f.FlightNumber,
                FromAirport = f.FromAirport,
                ToAirport = f.ToAirport,
                DepartureDateTime = f.DepartureDateTime,
                ArrivalDateTime = f.ArrivalDateTime,
                Price = f.Price,
                Baggage = f.BaggageAllowance,
                NonStop = f.IsNonStop,
                Rating = f.AverageRating,
                AircraftModel = f.Aircraft?.Model ?? "N/A"
            };
        }
    }
}