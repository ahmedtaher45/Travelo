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
<<<<<<< HEAD
        IRoomRepository Rooms { get; }
        IRoomBookingRepository RoomBookings { get; }
        IPaymentRepository Payment { get; }
        ICartRepository Cart { get; }

=======
        ICartRepository Cart { get; }
>>>>>>> origin/Reham

        Task<int> CompleteAsync ();
        Task SaveChangesAsync ();
    }
}
