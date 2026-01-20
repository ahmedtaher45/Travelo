using Stripe;
using Stripe.Checkout;
using Travelo.Application.Common.Responses;
using Travelo.Application.DTOs.Payment;
using Travelo.Application.Interfaces;
using Travelo.Domain.Models.Entities;

namespace Travelo.Application.Services.Payment
{
    public class PaymentServices : IPaymentServices
    {
        private readonly IPaymentRepository _payment;
        private readonly IHotelRepository _hotel;
        private readonly IRoomBookingRepository _roomBooking;

        public PaymentServices (IPaymentRepository payment, IHotelRepository hotel , IRoomBookingRepository roomBooking)
        {
            _payment=payment;
            _hotel=hotel;
            _roomBooking=roomBooking;
        }

        public async Task<GenericResponse<RoomBookingPaymentRes>> CreateRoomBookingPayment (RoomBookingPaymentReq req, string userId, string HTTPReq)
        {
            var hotel = await _hotel.GetHotelByIdAsync(req.HotelId);
            if (hotel==null)
            {
                return GenericResponse<RoomBookingPaymentRes>.FailureResponse("Hotel not found");
            }

            var payment = new Travelo.Domain.Models.Entities.Payment
            {
                UserId=userId,
                Amount=100,
                HotelId=req.HotelId,
                RoomId=req.RoomId,
                PaymentDate=DateTime.UtcNow,
                Status=PaymentStatus.Pending
            };
            await _payment.Add(payment);
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes=new List<string> { "card" },
                LineItems=new List<SessionLineItemOptions>(),
                Mode="payment",
                SuccessUrl=$"{HTTPReq}/api/User/Payment/Success/{payment.Id}",
                CancelUrl=$"{HTTPReq}/api/User/Payment/cancel",
            };
            options.LineItems.Add(new SessionLineItemOptions
            {
                PriceData=new SessionLineItemPriceDataOptions
                {
                    Currency="usd",
                    ProductData=new SessionLineItemPriceDataProductDataOptions
                    {
                        Name=hotel.Data.Name,
                        Description=hotel.Data.Description,
                        //    Name=Course.Title,
                        //    Description=Course.Description,
                        //    Images=new List<string> { $"{HTTPReq}/Images/Products/{Course.ImgeUrl}" }
                        //},
                    },
                    //UnitAmount=(long)(hotel.Data.Room.Price*100),
                },
                Quantity=1
            });
            var service = new SessionService();
            var session = service.Create(options);
            payment.PaymentId=session.Id;
            _payment.Update(payment);
            var response = new RoomBookingPaymentRes
            {
                Message="Payment initiated successfully",
                PaymentId=payment.Id.ToString(),
                Url=session.Url
            };
            return GenericResponse<RoomBookingPaymentRes>.SuccessResponse(response);
        }

        public async Task<GenericResponse<RoomBookingPaymentRes>>? HandelSuccessAsync (int paymentId)
        {
            var payment = await _payment.GetById(paymentId);
            if (payment==null)
            {
                return GenericResponse<RoomBookingPaymentRes>.FailureResponse("Payment not found");
            }
            payment.Status=PaymentStatus.Completed;
            var roombooking = new Travelo.Domain.Models.Entities.RoomBooking
            {
                UserId=payment.UserId,
                RoomId=payment.RoomId
            };
            var roobooking = new RoomBooking
            {
                RoomId=payment.RoomId,
                UserId=payment.UserId
            };
            await _roomBooking.Add(roobooking);
            _payment.Update(payment);

        }
    }
}
