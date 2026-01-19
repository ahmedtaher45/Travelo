using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travelo.Application.Common.Responses;
using Travelo.Application.DTOs.Review;
using Travelo.Application.Interfaces;
using Travelo.Domain.Models.Entities;
using Travelo.Domain.Models.Enums;
using Travelo.Infrastracture.Contexts;

namespace Travelo.Infrastracture.Repositories
{
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository
    {
        private readonly ApplicationDbContext _context;

        public ReviewRepository(ApplicationDbContext context) : base(context) {
            _context = context;
        }

        public async Task<GenericResponse<List<Review>>> GetHotelReviews(
            int hotelId,
            int? pageNum = null, int? pageSize = null)
        {
            var query = _set
                .Include(r=>r.User)
                .Include(r=>r.Hotel)
                .Where(r => r.HotelId == hotelId)
                .AsQueryable();

            if(pageNum.HasValue && pageSize.HasValue)
            {
                query = query
                    .Skip((pageNum.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value);
            }
            return GenericResponse<List<Review>>.SuccessResponse(
                await query.AsNoTracking().ToListAsync(), "Success");

        }

        public async Task<GenericResponse<List<Review>>> GetUserReviews(string userId, int? pageNum = null, int? pageSize = null)
        {
            var query = _set
                .Include(r => r.Hotel)
                .Where(r => r.UserId == userId)
                .AsQueryable();

            if (pageNum.HasValue && pageSize.HasValue)
            {
                query = query
                    .Skip((pageNum.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value);
            }
            return GenericResponse<List<Review>>.SuccessResponse(
              await query.AsNoTracking().ToListAsync(), "Success");
        }
   
        public async Task<GenericResponse<Review>> GetUserReviewForHotel(string userId, int hotelId)
        {
            var review =  await _set
                .Include(r => r.Hotel)
                .Include(r => r.User)
                .FirstOrDefaultAsync(r=> r.UserId == userId && r.HotelId == hotelId);

            return GenericResponse<Review>.SuccessResponse(review, "Success");
        }
    }
}
