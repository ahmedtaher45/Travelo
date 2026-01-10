namespace Travelo.Application.Interfaces
{
    public interface IUnitOfWork
    {
        IAuthRepository Auth { get; }
        Task<int> CompleteAsync ();
        Task SaveChangesAsync ();
    }
}
