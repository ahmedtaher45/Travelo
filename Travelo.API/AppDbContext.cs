using Microsoft.EntityFrameworkCore;
using Travelo.API.models;

namespace Travelo.API
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
       : base(options)
        {
        }
        public DbSet<Aircraft> Aircrafts { get; set; } 
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookingPrice> BookingsPrice { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<SearchFlight> SearchFlights { get; set; }
    }
}
