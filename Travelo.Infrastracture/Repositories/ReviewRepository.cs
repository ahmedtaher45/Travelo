using Microsoft.EntityFrameworkCore;
using Travelo.Application.Common.Responses;
using Travelo.Application.Interfaces;
using Travelo.Domain.Models.Entities;
using Travelo.Infrastracture.Contexts;

namespace Travelo.Infrastracture.Repositories
{
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository
    {
        private readonly ApplicationDbContext _context;

        public ReviewRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<GenericResponse<List<Review>>> GetHotelReviews(int hotelId, int? pageNum = null, int? pageSize = null)
        {
            var query = _set.Include(r => r.User)
                            .Include(r => r.Hotel)
                            .Where(r => r.HotelId == hotelId)
                            .AsQueryable();

            if (pageNum.HasValue && pageSize.HasValue)
                query = query.Skip((pageNum.Value - 1) * pageSize.Value)
                             .Take(pageSize.Value);

            return GenericResponse<List<Review>>.SuccessResponse(await query.AsNoTracking().ToListAsync(), "Success");
        }

        public async Task<GenericResponse<List<Review>>> GetFlightReviews(int flightId, int? pageNum = null, int? pageSize = null)
        {
            var query = _set.Include(r => r.User)
                            .Include(r => r.Flight)
                            .Include(r => r.Airline)
                            .Where(r => r.FlightId == flightId)
                            .AsQueryable();

            if (pageNum.HasValue && pageSize.HasValue)
                query = query.Skip((pageNum.Value - 1) * pageSize.Value)
                             .Take(pageSize.Value);

            return GenericResponse<List<Review>>.SuccessResponse(await query.AsNoTracking().ToListAsync(), "Success");
        }

        public async Task<GenericResponse<List<Review>>> GetAirlineReviews(int airlineId, int? pageNum = null, int? pageSize = null)
        {
            var query = _set.Include(r => r.User)
                            .Include(r => r.Airline)
                            .Where(r => r.AirlineId == airlineId)
                            .AsQueryable();

            if (pageNum.HasValue && pageSize.HasValue)
                query = query.Skip((pageNum.Value - 1) * pageSize.Value)
                             .Take(pageSize.Value);

            return GenericResponse<List<Review>>.SuccessResponse(await query.AsNoTracking().ToListAsync(), "Success");
        }

        public async Task<GenericResponse<Review>> GetUserReviewForEntity(string userId, int? hotelId = null, int? flightId = null, int? airlineId = null)
        {
            var review = await _set.Include(r => r.User)
                                   .Include(r => r.Hotel)
                                   .Include(r => r.Flight)
                                   .Include(r => r.Airline)
                                   .FirstOrDefaultAsync(r => r.UserId == userId
                                        && r.HotelId == hotelId
                                        && r.FlightId == flightId
                                        && r.AirlineId == airlineId);
            return GenericResponse<Review>.SuccessResponse(review, "Success");
        }

        public async Task<GenericResponse<List<Review>>> GetUserReviews(string userId, int? pageNum = null, int? pageSize = null)
        {
            var query = _set.Include(r => r.Hotel)
                            .Include(r => r.Flight)
                            .Include(r => r.Airline)
                            .Where(r => r.UserId == userId)
                            .AsQueryable();

            if (pageNum.HasValue && pageSize.HasValue)
                query = query.Skip((pageNum.Value - 1) * pageSize.Value)
                             .Take(pageSize.Value);

            return GenericResponse<List<Review>>.SuccessResponse(await query.AsNoTracking().ToListAsync(), "Success");
        }
    }

}
