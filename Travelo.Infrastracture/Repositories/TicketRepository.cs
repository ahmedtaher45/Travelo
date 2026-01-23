using Microsoft.EntityFrameworkCore;
using Travelo.Application.Interfaces;
using Travelo.Domain.Models.Entities;
using Travelo.Infrastracture.Contexts;

namespace Travelo.Infrastracture.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly ApplicationDbContext _context;

        public TicketRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Ticket ticket)
        {
            await _context.Ticket.AddAsync(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task<Ticket?> GetByIdAsync(int id)
        {
            return await _context.Ticket
                .Include(t => t.Booking)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Ticket?> GetByBookingIdAsync(int bookingId)
        {
            return await _context.Ticket
                .Include(t => t.Booking)
                .FirstOrDefaultAsync(t => t.BookingId == bookingId);
        }

        public async Task UpdateAsync(Ticket ticket)
        {
            _context.Ticket.Update(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Ticket ticket)
        {
            _context.Ticket.Remove(ticket);
            await _context.SaveChangesAsync();
        }
    }
}
