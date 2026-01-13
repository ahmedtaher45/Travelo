namespace Travelo.Application.Interfaces
{
    public interface IUnitOfWork
    {
         IAuthRepository Auth { get; }
         IHotelRepository Hotels { get; }

        Task<int> CompleteAsync();
        Task SaveChangesAsync();
    }
}
