using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travelo.Application.DTOs.Ticket;

namespace Travelo.Application.Services.Ticket
{
    public interface ITicketService
    {
        Task<TicketDto> CreateTicketAsync(CreateTicketDto dto);
        Task<TicketDto> GetTicketByBookingIdAsync(int bookingId);
        Task<TicketDto> GetTicketByIdAsync(int ticketId);
        Task<TicketDto> ConfirmTicketAsync(int ticketId);
        Task DeleteTicketAsync(int ticketId);
    }

}
