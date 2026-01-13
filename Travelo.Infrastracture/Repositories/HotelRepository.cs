using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travelo.Application.DTOs.Common;
using Travelo.Application.Interfaces;
using Travelo.Domain.Models.Entities;
using Travelo.Infrastracture.Contexts;

namespace Travelo.Infrastracture.Repositories
{
    public class HotelRepository : IHotelRepository
    {
        private readonly ApplicationDbContext _context;

        public HotelRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Hotel>> GetFeaturedHotelsAsync(PaginationRequest request)
        {
            return await _context.Hotels
                .Where(h => h.IsFeatured) 
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize) 
                .ToListAsync();
        }
    }
}
