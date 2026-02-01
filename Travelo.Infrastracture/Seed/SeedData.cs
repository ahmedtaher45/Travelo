using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Travelo.Domain.Models.Entities;
using Travelo.Infrastracture.Contexts;

namespace Travelo.Persistence.Seed
{
    public static class SeedData
    {
        public static void Seed (ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            //SeedUsers(userManager).Wait();
            SeedHotels(context).Wait();
            SeedRooms(context).Wait();
        }

        private static async Task SeedUsers (UserManager<ApplicationUser> userManager)
        {
            if (userManager.Users.Any()) return;

            var users = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    UserName = "john_doe",
                    Email = "john.doe@example.com",
                    PhoneNumber = "+1234567890"
                },
                new ApplicationUser
                {
                    UserName = "sara_smith",
                    Email = "sara.smith@example.com",
                    PhoneNumber = "+1987654321"
                }
            };

            foreach (var user in users)
            {
                await userManager.CreateAsync(user, "P@ssw0rd123");
            }
        }

        private static async Task SeedHotels (ApplicationDbContext context)
        {
            if (context.Hotels.Any()) return;

            var user1 = await context.Users.FirstOrDefaultAsync(u => u.UserName=="NormaMohsan");
            // var user2 = await context.Users.FirstOrDefaultAsync(u => u.UserName=="sara_smith");

            var city = new List<City>
            {
                new City
                {
                    Name = "tokyo",
                    Country = "Japan",
                    Description = "The bustling capital city of Japan, known for its modern architecture, vibrant culture, and delicious cuisine.",
                    ImageUrl = "tokyo.jpg"
                },
                new City
                {
                    Name = "Paris",
                    Country = "France",
                    Description= "The romantic capital city of France, famous for its iconic landmarks such as the Eiffel Tower, Louvre Museum, and Notre-Dame Cathedral.",
                    ImageUrl = "paris.jpg"
                },
                new City
                {
                    Name = "New York",
                    Country = "USA",
                    Description = "The largest city in the United States, known for its towering skyscrapers, diverse culture, and vibrant arts scene.",
                    ImageUrl = "newyork.jpg"
                }
            };
            await context.Cities.AddRangeAsync(city);
            var hotels = new List<Hotel>
            {
                new Hotel
                {
                    Name = "Grand Palace Hotel",
                    ResponsibleName = "John Doe",
                    Address = "123 Main Street",
                    Country = "Jordan",
                    CityId = 1,
                    UserId = null,
                    Latitude = 31.9539,
                    Longitude = 35.9106,
                    PricePerNight = 120,
                    Rating = 4.6,
                    ReviewsCount = 240,
                    ImageUrl = "data:image/jpeg;base64,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",
                    IsFeatured = true,
                    Description = "Luxury hotel in the heart of the city."
                },
                new Hotel
                {
                    Name = "Sea View Resort",
                    ResponsibleName = "Sara Smith",
                    Address = "Beach Road",
                    Country = "Jordan",
                    CityId = 1,
                    UserId = null,
                    Latitude = 31.9556,
                    Longitude = 35.9456,
                    PricePerNight = 150,
                    Rating = 4.8,
                    ReviewsCount = 340,
                    ImageUrl = "https://www.imghotels.com/wp-content/uploads/img-hotels-IADGV_006-Dusk-Exterior-home.jpg",
                    IsFeatured = true,
                    Description = "Amazing sea view and great amenities."
                }
            };

            await context.Hotels.AddRangeAsync(hotels);
            await context.SaveChangesAsync();
        }

        private static async Task SeedRooms (ApplicationDbContext context)
        {
            if (context.Rooms.Any()) return;

            var hotel1 = await context.Hotels.FirstOrDefaultAsync(h => h.Name=="Grand Palace Hotel");
            var hotel2 = await context.Hotels.FirstOrDefaultAsync(h => h.Name=="Sea View Resort");

            var rooms = new List<Room>
            {
                new Room
                {
                    Type = "Standard Room",
                    PricePerNight = 100,
                    Capacity = 2,
                    View = "City View",
                    ImageUrl = "https://example.com/room1.jpg",
                    IsAvailable = true,
                    HotelId = hotel1.Id,
                    BedType = "Queen Bed",
                    Size = 25
                },
                new Room
                {
                    Type = "Deluxe Room",
                    PricePerNight = 150,
                    Capacity = 3,
                    View = "Sea View",
                    ImageUrl = "https://example.com/room2.jpg",
                    IsAvailable = true,
                    HotelId = hotel1.Id,
                    BedType = "King Bed",
                    Size = 30
                },
                new Room
                {
                    Type = "Standard Room",
                    PricePerNight = 120,
                    Capacity = 2,
                    View = "Sea View",
                    ImageUrl = "https://example.com/room3.jpg",
                    IsAvailable = true,
                    HotelId = hotel2.Id,
                    BedType = "Queen Bed",
                    Size = 25
                },
                new Room
                {
                    Type = "Suite",
                    PricePerNight = 200,
                    Capacity = 4,
                    View = "Ocean View",
                    ImageUrl = "https://example.com/room4.jpg",
                    IsAvailable = true,
                    HotelId = hotel2.Id,
                    BedType = "King Bed",
                    Size = 45
                }
            };

            await context.Rooms.AddRangeAsync(rooms);
            await context.SaveChangesAsync();
        }
    }
}
