using Microsoft.EntityFrameworkCore;
using Travelo.Application.Interfaces;
using Travelo.Domain.Models.Entities;
using Travelo.Infrastracture.Contexts;

namespace Travelo.Infrastracture.Repositories
{
    public class FlightRepository : GenericRepository<Flight>, IFlightRepository
    {
        private readonly ApplicationDbContext _context;

        public FlightRepository (ApplicationDbContext context) : base(context)
        {
            _context=context;
        }

        public IQueryable<Flight> GetAllQueryable()
        {
            return _context.Flights
                .Include(f => f.Airline)
                .Include(f => f.Aircraft)
                .Where(f => !f.IsDeleted);
        }

        public async Task<Flight?> GetByIdAsync (int id)
        {
            return await GetAllQueryable().FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task AddAsync (Flight flight)
        {
            flight.CreatedOn=DateTime.UtcNow;
            context.Flights.Add(flight);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync (Flight flight)
        {
            flight.ModifiedOn=DateTime.UtcNow;
            context.Flights.Update(flight);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync (int id)
        {
            var flight = await context.Flights.FindAsync(id);
            if (flight!=null)
            {
                flight.Delete();
                flight.ModifiedOn=DateTime.UtcNow;
                await context.SaveChangesAsync();
            }
        }
    }

}
