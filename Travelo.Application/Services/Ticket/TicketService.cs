using Travelo.Application.DTOs.Ticket;
using Travelo.Application.Interfaces;
using Travelo.Domain.Models.Entities;
using Travelo.Domain.Models.Enums;

namespace Travelo.Application.Services.Ticket
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IBookingRepository _bookingRepository;

        public TicketService(ITicketRepository ticketRepository, IBookingRepository bookingRepository)
        {
            _ticketRepository = ticketRepository;
            _bookingRepository = bookingRepository;
        }

        public async Task<TicketDto> CreateTicketAsync(CreateTicketDto dto)
        {
            var booking = await _bookingRepository.GetByIdAsync(dto.BookingId);
            if (booking == null)
                throw new Exception("Booking not found");

            if (booking.Status != Travelo.Domain.Models.Enums.BookingStatus.Confirmed)
                throw new Exception("Booking must be confirmed before creating a ticket");

            var ticket = new Travelo.Domain.Models.Entities.Ticket
            {
                BookingId = dto.BookingId,
                TicketNumber = Guid.NewGuid().ToString().Substring(0, 8),
                SeatNumber = dto.SeatNumber,
                Gate = dto.Gate,
                Barcode = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 12),
                Status = TicketStatus.Pending
            };

            await _ticketRepository.AddAsync(ticket);

            return MapToDto(ticket);
        }

        public async Task<TicketDto> GetTicketByBookingIdAsync(int bookingId)
        {
            var ticket = await _ticketRepository.GetByBookingIdAsync(bookingId);
            if (ticket == null) throw new Exception("Ticket not found");

            return MapToDto(ticket);
        }

        public async Task<TicketDto> GetTicketByIdAsync(int ticketId)
        {
            var ticket = await _ticketRepository.GetByIdAsync(ticketId);
            if (ticket == null) throw new Exception("Ticket not found");

            return MapToDto(ticket);
        }

        public async Task<TicketDto> ConfirmTicketAsync(int ticketId)
        {
            var ticket = await _ticketRepository.GetByIdAsync(ticketId);
            if (ticket == null) throw new Exception("Ticket not found");

            ticket.Status = TicketStatus.Confirmed;
            await _ticketRepository.UpdateAsync(ticket);

            return MapToDto(ticket);
        }

        public async Task DeleteTicketAsync(int ticketId)
        {
            var ticket = await _ticketRepository.GetByIdAsync(ticketId);
            if (ticket == null) throw new Exception("Ticket not found");

            await _ticketRepository.DeleteAsync(ticket);
        }

        private TicketDto MapToDto(Travelo.Domain.Models.Entities.Ticket ticket)
        {
            return new TicketDto
            {
                Id = ticket.Id,
                BookingId = ticket.BookingId,
                TicketNumber = ticket.TicketNumber,
                SeatNumber = ticket.SeatNumber,
                Gate = ticket.Gate,
                Barcode = ticket.Barcode,
                Status = ticket.Status
            };
        }
    }
}
