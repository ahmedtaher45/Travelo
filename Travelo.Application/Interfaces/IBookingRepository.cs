using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travelo.Domain.Models.Entities;

namespace Travelo.Application.Interfaces
{
    public interface IBookingRepository
    {
        Task AddAsync(Booking booking);
        Task<Booking?> GetByIdAsync(int id);
        Task SaveChangesAsync();
    }

}
