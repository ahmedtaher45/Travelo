using Microsoft.EntityFrameworkCore;
using Travelo.Domain.Models.Entities;

namespace Travelo.Infrastracture.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<City> Cities { get; set; }
    }
}
