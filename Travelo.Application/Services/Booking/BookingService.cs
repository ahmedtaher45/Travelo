using Travelo.Application.DTOs.Booking;
using Travelo.Application.Interfaces;
using Travelo.Domain.Models.Enums;

namespace Travelo.Application.Services.Booking
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IFlightRepository _flightRepository;

        public BookingService(
            IBookingRepository bookingRepository,
            IFlightRepository flightRepository)
        {
            _bookingRepository = bookingRepository;
            _flightRepository = flightRepository;
        }

        public async Task<BookingDto> CreateFlightBookingAsync(CreateBookingDto dto)
        {
            var flight = await _flightRepository.GetByIdAsync(dto.FlightId);
            if (flight == null)
                throw new Exception("Flight not found");

            var booking = new Travelo.Domain.Models.Entities.Booking
            {
                FlightId = flight.Id,
                Status = BookingStatus.PendingPayment,
                TotalPrice = flight.Price
            };

            await _bookingRepository.AddAsync(booking);
            await _bookingRepository.SaveChangesAsync();

            return MapToDto(booking);
        }

        public async Task<BookingDto> ConfirmBookingAsync(int bookingId)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);
            if (booking == null)
                throw new Exception("Booking not found");

            if (booking.Status == BookingStatus.Cancelled)
                throw new Exception("Booking is cancelled");

            if (booking.Status != BookingStatus.PendingPayment)
                throw new Exception("Booking cannot be confirmed");


            booking.Status = BookingStatus.Confirmed;

            await _bookingRepository.SaveChangesAsync();

            return MapToDto(booking);
        }

        public async Task CancelBookingAsync(int bookingId)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);
            if (booking == null)
                throw new Exception("Booking not found");

            booking.Status = BookingStatus.Cancelled;
            await _bookingRepository.SaveChangesAsync();
        }

        private BookingDto MapToDto(Travelo.Domain.Models.Entities.Booking b)
        {
            return new BookingDto
            {
                Id = b.Id,
                FlightId = b.FlightId,
                Status = b.Status,
                TotalPrice = b.TotalPrice,
                TicketId = b.Ticket?.Id
            };
        }
    }
}
