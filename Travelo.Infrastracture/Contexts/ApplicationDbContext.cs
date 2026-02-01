using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Travelo.Domain.Models.Entites;
using Travelo.Domain.Models.Entities;
using Travelo.Domain.Models.Enums;

namespace Travelo.Infrastracture.Contexts
{
    public class ApplicationDbContext (DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<Aircraft> Aircrafts { get; set; }
        public DbSet<Airline> Airlines { get; set; }
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookingPrice> BookingPrices { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<MenuCategory> MenuCategories { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<SupportTicket> SupportTickets { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<ThingToDo> ThingsToDo { get; set; }
        public DbSet<RoomBooking> RoomBookings { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Ticket> Ticket { get; set; }




        override protected void OnModelCreating (ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Airline>().HasData(
                new Airline
                {
                    Id = 1,
                    Name = "EgyptAir",
                    LogoUrl = "https://example.com/logos/egyptair.png"
                },
                new Airline
                {
                    Id = 2,
                    Name = "FlyDubai",
                    LogoUrl = "https://example.com/logos/flydubai.png"
                }
            );

            builder.Entity<Aircraft>().HasData(
                new Aircraft
                {
                    Id = 1,
                    Model = "Airbus A320",
                    CountOfSeats = 180
                },
                new Aircraft
                {
                    Id = 2,
                    Model = "Airbus A330",
                    CountOfSeats = 250
                },
                new Aircraft
                {
                    Id = 3,
                    Model = "Boeing 737-800",
                    CountOfSeats = 189
                },
                new Aircraft
                {
                    Id = 4,
                    Model = "Boeing 777",
                    CountOfSeats = 396
                }
            );

            builder.Entity<Flight>().HasData(
                new Flight
                {
                    Id = 5,
                    AirlineId = 1, // EgyptAir
                    AircraftId = 1,
                    FlightNumber = "MS101",
                    FromAirport = "CAI",
                    ToAirport = "DXB",
                    DepartureDateTime = new DateTime(2026, 2, 10, 10, 00, 00),
                    ArrivalDateTime = new DateTime(2026, 2, 10, 14, 00, 00),
                    Price = 450,
                    AvailableSeats = 50,
                    Class = FlightClass.Economy,
                    IsNonStop = true,
                    BaggageAllowance = "20kg",
                    AverageRating = 4.5m,
                    ReviewsCount = 120,
                    CreatedOn = new DateTime(2026, 1, 29)
                },
                new Flight
                {
                    Id = 6,
                    AirlineId = 1,
                    AircraftId = 2,
                    FlightNumber = "MS202",
                    FromAirport = "DXB",
                    ToAirport = "CAI",
                    DepartureDateTime = new DateTime(2026, 2, 20, 15, 00, 00),
                    ArrivalDateTime = new DateTime(2026, 2, 20, 19, 00, 00),
                    Price = 480,
                    AvailableSeats = 60,
                    Class = FlightClass.Economy,
                    IsNonStop = true,
                    BaggageAllowance = "25kg",
                    AverageRating = 4.6m,
                    ReviewsCount = 95,
                    CreatedOn = new DateTime(2026, 1, 29)
                },
                new Flight
                {
                    Id = 7,
                    AirlineId = 2, // FlyDubai
                    AircraftId = 3,
                    FlightNumber = "FD303",
                    FromAirport = "CAI",
                    ToAirport = "JED",
                    DepartureDateTime = new DateTime(2026, 2, 12, 09, 30, 00),
                    ArrivalDateTime = new DateTime(2026, 2, 12, 12, 00, 00),
                    Price = 300,
                    AvailableSeats = 80,
                    Class = FlightClass.Economy,
                    IsNonStop = false,
                    BaggageAllowance = "20kg",
                    AverageRating = 4.1m,
                    ReviewsCount = 60,
                    CreatedOn = new DateTime(2026, 1, 29)
                },
                new Flight
                {
                    Id = 8,
                    AirlineId = 2,
                    AircraftId = 4,
                    FlightNumber = "FD404",
                    FromAirport = "CAI",
                    ToAirport = "DXB",
                    DepartureDateTime = new DateTime(2026, 2, 11, 20, 00, 00),
                    ArrivalDateTime = new DateTime(2026, 2, 12, 01, 00, 00),
                    Price = 520,
                    AvailableSeats = 40,
                    Class = FlightClass.Business,
                    IsNonStop = true,
                    BaggageAllowance = "35kg",
                    AverageRating = 4.8m,
                    ReviewsCount = 200,
                    CreatedOn = new DateTime(2026, 1, 29)
                }
            );

            builder.Entity<Booking>()
                   .HasOne(b => b.Ticket)
                   .WithOne(t => t.Booking)
                   .HasForeignKey<Ticket>(t => t.BookingId);
            builder.Entity<ApplicationUser>().ToTable("Users");
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");

            builder.Ignore<IdentityUserLogin<string>>();
            builder.Ignore<IdentityUserClaim<string>>();
            builder.Ignore<IdentityUserToken<string>>();

            builder.Entity<ApplicationUser>()
                .HasOne(a => a.Hotel)
                .WithOne(a => a.User)
                .HasForeignKey<Hotel>(a => a.UserId);

            builder.Entity<ApplicationUser>()
              .HasMany(u => u.SupportTickets)
              .WithOne(p => p.User)
              .HasForeignKey(p => p.userId)
              .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<ApplicationUser>()
                .HasOne(a => a.Restaurant)
                .WithOne(a => a.User)
                .HasForeignKey<Restaurant>(a => a.UserId);

            builder.Entity<Payment>()
               .HasOne(p => p.Hotel)
               .WithMany()
               .HasForeignKey(p => p.HotelId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Payment>()
                .HasOne(p => p.Room)
                .WithMany()
                .HasForeignKey(p => p.RoomId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Payment>()
                .HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Room>()
                .HasOne(r => r.Hotel)
                .WithMany(h => h.Rooms)
                .HasForeignKey(r => r.HotelId)
                .OnDelete(DeleteBehavior.Restrict);
            var decimalProperties = builder.Model.GetEntityTypes()
                   .SelectMany(t => t.GetProperties())
                   .Where(p => p.ClrType==typeof(decimal)||p.ClrType==typeof(decimal?));

            foreach (var property in decimalProperties)
            {
                property.SetPrecision(18);
                property.SetScale(2);
            }
        }
    }
}
