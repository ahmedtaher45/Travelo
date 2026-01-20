using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Travelo.Application.Interfaces;
using Travelo.Domain.Models.Entities;
using Travelo.Infrastracture.Contexts;

namespace Travelo.Infrastracture.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        private readonly Dictionary<Type, object> _repositories;

        public UnitOfWork(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IEmailSender emailSender,
            IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _emailSender = emailSender;
            _configuration = configuration;

            _repositories = new Dictionary<Type, object>();

            Auth = new AuthRepository(_userManager, _context, _configuration, _emailSender);
            Hotels = new HotelRepository(_context);
            Cities = new CityRepository(_context);
            Reviews = new ReviewRepository(_context);
            Menu = new MenuRepository(_context);
        }

        public IAuthRepository Auth { get; private set; }
        public IHotelRepository Hotels { get; private set; }
        public ICityRepository Cities { get; private set; }
        public IReviewRepository Reviews { get; private set; }
        public IMenuRepository Menu { get; private set; }

        public IGenericRepository<T> Repository<T>() where T : class
        {
            var type = typeof(T);

            if (!_repositories.ContainsKey(type))
            {
                _repositories[type] = new GenericRepository<T>(_context);
            }

            return (IGenericRepository<T>)_repositories[type];
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}