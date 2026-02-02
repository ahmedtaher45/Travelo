using Travelo.Application.Common.Responses;
using Travelo.Application.DTOs.Payment;

namespace Travelo.Application.Services.Payment
{
    public interface IPaymentServices
    {
        Task<GenericResponse<PaymentRes>> CreateRoomBookingPayment (RoomBookingPaymentReq req, string userId, string HTTPReq);
        Task<GenericResponse<PaymentRes>> HandleSuccessAsync (int paymentId);
        Task<GenericResponse<PaymentRes>> CartCheckout (CheckoutReq req, string userId, string HTTPReq);
        Task<GenericResponse<PaymentRes>> HandelCartSuccessAsync (int orderId);
        Task<GenericResponse<PaymentRes>> FlightBookingPayment (FlightPaymentRequest req, string userId, string HTTPReq);
    }
}
