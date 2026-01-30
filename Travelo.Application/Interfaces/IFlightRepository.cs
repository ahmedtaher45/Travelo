using Travelo.Domain.Models.Entities;

namespace Travelo.Application.Interfaces
{
    public interface IFlightRepository : IGenericRepository<Flight>
    {
        Task<List<Flight>> GetAllAsync ();
        Task<Flight?> GetByIdAsync (int id);
        Task AddAsync (Flight flight);
        Task UpdateAsync (Flight flight);
        Task DeleteAsync (int id);
    }

}
