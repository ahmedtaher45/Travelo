namespace Travelo.Application.Interfaces
{
    public interface IUnitOfWork
    {
        IAuthRepository Auth { get; }
        IHotelRepository Hotels { get; }
        ICityRepository Cities { get; }
        IGenericRepository<T> Repository<T> () where T : class;
        IReviewRepository Reviews { get; }
        IMenuRepository Menu { get; }
        object Payment { get; }

        Task<int> CompleteAsync ();
        Task SaveChangesAsync ();
    }
}
