using Microsoft.EntityFrameworkCore;
using Travelo.Application.Interfaces;
using Travelo.Domain.Models.Entities;
using Travelo.Infrastracture.Contexts;

namespace Travelo.Infrastracture.Repositories
{
    public class FlightRepository : IFlightRepository
    {
        private readonly ApplicationDbContext _context;

        public FlightRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Flight>> GetAllAsync()
        {
            return await _context.Flights
                .Where(f => !f.IsDeleted)
                .ToListAsync();
        }

        public async Task<Flight?> GetByIdAsync(int id)
        {
            return await _context.Flights
                .FirstOrDefaultAsync(f => f.Id == id && !f.IsDeleted);
        }

        public async Task AddAsync(Flight flight)
        {
            flight.CreatedOn = DateTime.UtcNow;
            _context.Flights.Add(flight);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Flight flight)
        {
            flight.ModifiedOn = DateTime.UtcNow;
            _context.Flights.Update(flight);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var flight = await _context.Flights.FindAsync(id);
            if (flight != null)
            {
                flight.Delete();
                flight.ModifiedOn = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

    }

}
