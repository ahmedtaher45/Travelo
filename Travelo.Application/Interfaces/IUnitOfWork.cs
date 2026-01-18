using System.Threading.Tasks;

namespace Travelo.Application.Interfaces
{
    public interface IUnitOfWork
    {
        IAuthRepository Auth { get; }
        IHotelRepository Hotels { get; }
        ICityRepository Cities { get; }

        Task<int> CompleteAsync ();
        Task SaveChangesAsync ();
    }
}
