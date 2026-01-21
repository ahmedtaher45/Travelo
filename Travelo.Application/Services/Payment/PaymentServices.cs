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
        private readonly IRoomRepository _roomRepository;
        private readonly IUnitOfWork unitOfWork;

        public PaymentServices (IPaymentRepository payment, IHotelRepository hotel, IRoomBookingRepository roomBooking, IRoomRepository roomRepository, IUnitOfWork unitOfWork)
        {
            _payment=payment;
            _hotel=hotel;
            _roomBooking=roomBooking;
            _roomRepository=roomRepository;
            this.unitOfWork=unitOfWork;
        }

        public async Task<GenericResponse<RoomBookingPaymentRes>> CreateRoomBookingPayment (RoomBookingPaymentReq req, string userId, string HTTPReq)
        {
            var hotelResponse = await _hotel.GetById(req.HotelId);
            var room = await _roomRepository.GetById(req.RoomId);

            if (hotelResponse==null||hotelResponse==null)
                return GenericResponse<RoomBookingPaymentRes>.FailureResponse("Hotel not found");

            if (room==null)
                return GenericResponse<RoomBookingPaymentRes>.FailureResponse("Room not found");
            var payment = new Domain.Models.Entities.Payment
            {
                UserId=userId,
                RoomId=req.RoomId,
                Amount=room.PricePerNight,
                Status=PaymentStatus.Pending,
            };
            await _payment.Add(payment);
            int numberOfNights = (req.CheckOutDate-req.CheckInDate).Days;
            if (numberOfNights<=0) numberOfNights=1;

            decimal totalAmount = room.PricePerNight*numberOfNights;


            var options = new SessionCreateOptions
            {
                PaymentMethodTypes=new List<string> { "card" },
                LineItems=new List<SessionLineItemOptions>
            {
            new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    Currency = "usd",
                    UnitAmount = (long)(totalAmount * 100),
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = hotelResponse.Name,
                        Description = $"Room Type: {room.Type}"
                    }
                },
                Quantity = 1
            }
        },
                Mode="payment",
                SuccessUrl=$"{HTTPReq}/api/User/Payment/Success/{payment.Id}",
                CancelUrl=$"{HTTPReq}/api/User/Payment/cancel",
            };

            var service = new SessionService();
            Session session = await service.CreateAsync(options); // Use Async

            payment.PaymentId=session.Id;
            _payment.Update(payment);
            await unitOfWork.SaveChangesAsync();
            return GenericResponse<RoomBookingPaymentRes>.SuccessResponse(new RoomBookingPaymentRes
            {
                Message="Payment initiated successfully",
                PaymentId=payment.Id.ToString(),
                Url=session.Url
            });
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
            await unitOfWork.SaveChangesAsync();
            return GenericResponse<RoomBookingPaymentRes>.SuccessResponse(new RoomBookingPaymentRes
            {
                Message="Payment successful"
            });

        }
    }
}
