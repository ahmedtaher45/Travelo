using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travelo.Application.Interfaces;
using Travelo.Domain.Models.Entities;
using Travelo.Infrastracture.Contexts;

namespace Travelo.Infrastracture.Repositories
{
    public class MenuRepository : IMenuRepository
    {
        private readonly ApplicationDbContext _context;

        public MenuRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<MenuCategory>> GetMenu(int restaurantId)
        {
            return await _context.MenuCategories
                .Where(x => x.RestaurantId == restaurantId && !x.IsDeleted)
                .Include(x => x.items)
                .ToListAsync();
        }
    }
}
