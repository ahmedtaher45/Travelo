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

        public UnitOfWork (
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IEmailSender emailSender,
            IConfiguration configuration)
        {
            _context=context;
            _userManager=userManager;
            _emailSender=emailSender;
            _configuration=configuration;

            _repositories=new Dictionary<Type, object>();

<<<<<<< HEAD
            Auth=new AuthRepository(_userManager, _context, _configuration, _emailSender);
            Hotels=new HotelRepository(_context);
            Cities=new CityRepository(_context);
            Reviews=new ReviewRepository(_context);
            Menu=new MenuRepository(_context);
            Rooms=new RoomRepository(_context);
            RoomBookings=new RoomBookingRepository(_context);
            Payment=new PaymentRepository(_context);
            Cart=new CartRepository(_context);
=======
            Auth = new AuthRepository(_userManager, _context, _configuration, _emailSender);
            Hotels = new HotelRepository(_context);
            Cities = new CityRepository(_context);
            Reviews = new ReviewRepository(_context);
            Menu = new MenuRepository(_context);
            Cart = new CartRepository(_context);
>>>>>>> origin/Reham
        }


        public IAuthRepository Auth { get; private set; }
        public IHotelRepository Hotels { get; private set; }
        public ICityRepository Cities { get; private set; }
        public IReviewRepository Reviews { get; private set; }
        public IMenuRepository Menu { get; private set; }

<<<<<<< HEAD
        public IRoomRepository Rooms { get; private set; }
        public IRoomBookingRepository RoomBookings { get; private set; }
        public IPaymentRepository Payment { get; private set; }


        public ICartRepository Cart { get; private set; }

        public IGenericRepository<T> Repository<T> () where T : class
=======
        public ICartRepository Cart { get; private set; }

        public IGenericRepository<T> Repository<T>() where T : class
>>>>>>> origin/Reham
        {
            var type = typeof(T);

            if (!_repositories.ContainsKey(type))
            {
                _repositories[type]=new GenericRepository<T>(_context);
            }

            return (IGenericRepository<T>)_repositories[type];
        }

        public async Task<int> CompleteAsync ()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync ()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose ()
        {
            _context.Dispose();
        }
    }
}