using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travelo.Domain.Models.Entities;

namespace Travelo.Application.Interfaces
{
    public interface IFlightRepository
    {
        Task<List<Flight>> GetAllAsync();
        Task<Flight?> GetByIdAsync(int id);
        Task AddAsync(Flight flight);
        Task UpdateAsync(Flight flight);
        Task DeleteAsync(int id);
    }

}
