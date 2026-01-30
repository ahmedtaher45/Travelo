using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Travelo.Domain.Models.Entites;
using Travelo.Domain.Models.Entities;

namespace Travelo.Infrastracture.Contexts
{
    public class ApplicationDbContext (DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<Aircraft> Aircrafts { get; set; }
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
        public DbSet<FlightBooking> FlightBookings { get; set; }

        override protected void OnModelCreating (ModelBuilder builder)
        {
            base.OnModelCreating(builder);

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
