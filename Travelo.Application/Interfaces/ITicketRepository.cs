using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travelo.Domain.Models.Entities;

namespace Travelo.Application.Interfaces
{
    public interface ITicketRepository
    {
        Task AddAsync(Ticket ticket);
        Task<Ticket?> GetByIdAsync(int id);
        Task<Ticket?> GetByBookingIdAsync(int bookingId);
        Task UpdateAsync(Ticket ticket);
        Task DeleteAsync(Ticket ticket);
    }

}
