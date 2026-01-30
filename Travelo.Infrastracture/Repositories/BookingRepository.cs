using Microsoft.EntityFrameworkCore;
using Travelo.Application.Interfaces;
using Travelo.Domain.Models.Entities;
using Travelo.Infrastracture.Contexts;

namespace Travelo.Infrastracture.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly ApplicationDbContext _context;

        public BookingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Booking booking)
        {
            booking.CreatedOn = DateTime.UtcNow;
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
        }

        public async Task<Booking?> GetByIdAsync(int id)
        {
            return await _context.Bookings
                .Include(b => b.Ticket)
                .FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
