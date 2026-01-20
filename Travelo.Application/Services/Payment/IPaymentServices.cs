using Travelo.Application.Common.Responses;
using Travelo.Application.DTOs.Payment;

namespace Travelo.Application.Services.Payment
{
    public interface IPaymentServices
    {
        Task<GenericResponse<DTOs.Payment.RoomBookingPaymentRes>> CreateRoomBookingPayment (DTOs.Payment.RoomBookingPaymentReq req, string userId, string HTTPReq);
        Task<GenericResponse<RoomBookingPaymentRes>>? HandelSuccessAsync (int paymentId);


    }
}
