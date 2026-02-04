using Travelo.Application.Common.Responses;
using Travelo.Application.DTOs.Booking;
using Travelo.Application.DTOs.Payment;

namespace Travelo.Application.Services.Booking
{
    public interface IBookingService
    {
        Task<BookingDetailsDto?> GetDetailsAsync (int id);
        Task<List<TripDto>> GetMyTripsAsync (string userId);
        Task<BookingDto> CreateFlightBookingAsync (CreateBookingDto dto);
        Task<BookingDto> ConfirmBookingAsync (int bookingId);
        Task CancelBookingAsync (int bookingId);
        Task<GenericResponse<IEnumerable<BookingDetailsDto>>> GetAllFlightBookingAsync ();
        Task<GenericResponse<IEnumerable<RoomBookingPaymentRes>>> GetAllRoomBookingAsync ();
        Task<GenericResponse<IEnumerable<RoomBookingPaymentRes>>> GetHotelRoomBookingAsync (int hotilId);





    }

}
