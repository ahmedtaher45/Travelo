namespace Travelo.Application.Interfaces
{
    public interface IUnitOfWork
    {
        IAuthRepository Auth { get; }
        IHotelRepository Hotels { get; }
        ICityRepository Cities { get; }
        IReviewRepository Reviews { get; }
        IMenuRepository Menu { get; }
        IRoomRepository Rooms { get; }
        IRoomBookingRepository RoomBookings { get; }
        IPaymentRepository Payment { get; }
        ISupportTicket SupportTicket { get; }
        ICartRepository Cart { get; }
        IOrderRepository OrderRepository { get; }
        IOrderItemRepository OrderItems { get; }
        IFlightRepository Flights { get; }
        IFlightBookingRepository FlightBookings { get; }
        IRestaurantRepository Restaurant { get; }


        IGenericRepository<T> Repository<T> () where T : class;

        Task<int> CompleteAsync ();
        Task SaveChangesAsync ();
    }
}