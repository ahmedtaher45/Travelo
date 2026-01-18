using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Travelo.Application.Interfaces;
using Travelo.Application.Services.FileService;
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


        public UnitOfWork (ApplicationDbContext context, UserManager<ApplicationUser> userManager, IEmailSender emailSender, IConfiguration configuration, IFileService fileService)
        {
            _context=context;
            _userManager=userManager;
            _emailSender=emailSender;
            _configuration=configuration;
            Auth=new AuthRepository(_userManager, _context, _configuration, _emailSender);
            Hotels=new HotelRepository(_context);
            Cities=new CityRepository(_context);
            _context=context;
            _userManager=userManager;
            // Auth = new AuthRepository(_context, _userManager);
            Hotels=new HotelRepository(_context);
        }

        public IAuthRepository Auth { get; private set; }

        public IHotelRepository Hotels { get; private set; }

        public ICityRepository Cities { get; private set; }

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
