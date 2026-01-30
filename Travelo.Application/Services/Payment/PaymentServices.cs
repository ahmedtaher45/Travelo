using Microsoft.EntityFrameworkCore;
using Stripe.Checkout;
using Travelo.Application.Common.Responses;
using Travelo.Application.DTOs.Payment;
using Travelo.Application.Interfaces;
using Travelo.Domain.Models.Entities;
using Travelo.Domain.Models.Enums;

namespace Travelo.Application.Services.Payment
{
    public class PaymentServices : IPaymentServices
    {
        private readonly IPaymentRepository _payment;
        private readonly IHotelRepository _hotel;
        private readonly IRoomBookingRepository _roomBooking;
        private readonly IRoomRepository _roomRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICartRepository _cartRepository;

        public PaymentServices (IPaymentRepository payment, IHotelRepository hotel, IRoomBookingRepository roomBooking, IRoomRepository roomRepository, IUnitOfWork unitOfWork, ICartRepository cartRepository)
        {
            _payment=payment;
            _hotel=hotel;
            _roomBooking=roomBooking;
            _roomRepository=roomRepository;
            this.unitOfWork=unitOfWork;
            _cartRepository=cartRepository;
        }

        public async Task<GenericResponse<PaymentRes>> CartCheckout (CheckoutReq req, string userId, string HTTPReq)
        {
            var cart = await _cartRepository.GetCartByUserId(userId);
            if (cart==null||cart.CartItems==null||!cart.CartItems.Any())
            {
                return GenericResponse<PaymentRes>.FailureResponse("Cart is empty");
            }
            decimal tax = cart.TotalPrice-cart.SubTotal;

            Order order = new Order
            {
                UserId=userId,
                SubTotal=cart.SubTotal,
                Tax=tax,
                Total=cart.TotalPrice,
                PaymentType=req.PaymentType,
                Status=Domain.Models.Enums.OrderStatus.Pending
            };
            await unitOfWork.OrderRepository.Add(order);
            await unitOfWork.SaveChangesAsync();
            if (req.PaymentType==Domain.Models.Enums.PaymentType.Cash)
            {
                return GenericResponse<PaymentRes>.SuccessResponse(new PaymentRes
                {
                    Message="Order placed successfully with Cash payment",
                    PaymentId=order.Id.ToString(),
                    Url=null
                });
            }
            if (req.PaymentType==Domain.Models.Enums.PaymentType.Card||req.PaymentType==Domain.Models.Enums.PaymentType.Wallet)
            {
                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes=new List<string> { "card" },
                    LineItems=new List<SessionLineItemOptions>(),
                    Mode="payment",
                    SuccessUrl=$"{HTTPReq}/api/User/Payment/Success/{order.Id}",
                    CancelUrl=$"{HTTPReq}/api/User/Payment/cancel",
                };
                foreach (var item in cart.CartItems)
                {
                    options.LineItems.Add(new SessionLineItemOptions
                    {
                        PriceData=new SessionLineItemPriceDataOptions
                        {
                            Currency="usd",
                            //UnitAmount=(long)(item.Pr),
                            UnitAmount=(long)(item.Product.Price*100),
                            ProductData=new SessionLineItemPriceDataProductDataOptions
                            {
                                Name=item.Product.Name,
                                Description=item.Product.Description
                            }
                        },
                        Quantity=item.Quantity
                    });
                }
                var service = new SessionService();
                Session session = await service.CreateAsync(options);
                order.PaymentId=session.Id;
                unitOfWork.OrderRepository.Update(order);
                await unitOfWork.SaveChangesAsync();
                return GenericResponse<PaymentRes>.SuccessResponse(new PaymentRes
                {
                    Message="Payment initiated successfully",
                    PaymentId=order.Id.ToString(),
                    Url=session.Url
                });
            }
            return GenericResponse<PaymentRes>.FailureResponse("Invalid payment type");

        }

        public async Task<GenericResponse<PaymentRes>> CreateRoomBookingPayment (RoomBookingPaymentReq req, string userId, string HTTPReq)
        {
            var hotelResponse = await _hotel.GetById(req.HotelId);
            var room = await _roomRepository.GetById(req.RoomId);

            if (hotelResponse==null)
                return GenericResponse<PaymentRes>.FailureResponse("Hotel not found");

            if (room==null)
                return GenericResponse<PaymentRes>.FailureResponse("Room not found");
            var payment = new Domain.Models.Entities.Payment
            {
                UserId=userId,
                RoomId=req.RoomId,
                HotelId=req.HotelId,
                Amount=room.PricePerNight,
                Status=PaymentStatus.Pending,
                CheckInDate=req.CheckInDate,
                CheckOutDate=req.CheckOutDate,
                PaymentFor=PaymentFor.Room,
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
                SuccessUrl=$"{HTTPReq}/api/Payment/OrderSuccess/{payment.Id}",
                CancelUrl=$"{HTTPReq}/api/Payment/cancel",
            };

            var service = new SessionService();
            Session session = await service.CreateAsync(options); // Use Async

            payment.PaymentId=session.Id;
            _payment.Update(payment);
            await unitOfWork.SaveChangesAsync();
            return GenericResponse<PaymentRes>.SuccessResponse(new PaymentRes
            {
                Message="Payment initiated successfully",
                PaymentId=payment.Id.ToString(),
                Url=session.Url
            });
        }

        public async Task<GenericResponse<PaymentRes>> FlightBookingPayment (FlightPaymentRequest req, string userId, string HTTPReq)
        {
            var flight = await unitOfWork.Flights.GetById((int)req.FlightId, q => q.Include(f => f.Aircraft));
            if (flight==null)
            {
                return GenericResponse<PaymentRes>.FailureResponse("Flight not found");
            }
            if (flight.Aircraft.CountOfSeats<req.numberOfTickets)
            {
                return GenericResponse<PaymentRes>.FailureResponse("Not enough seats available");
            }
            if (req.numberOfTickets<=0)
            {
                return GenericResponse<PaymentRes>.FailureResponse("Number of tickets must be greater than zero");
            }
            var payment = new Domain.Models.Entities.Payment
            {
                UserId=userId,
                Amount=flight.Price*req.numberOfTickets,
                Status=PaymentStatus.Pending,
                PaymentFor=PaymentFor.Flight,
                FlightId=req.FlightId,
                NumberOfTickets=req.numberOfTickets
            };
            await _payment.Add(payment);
            if (req.PaymentType==PaymentType.Card)
            {
                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes=new List<string> { "card" },
                    LineItems=new List<SessionLineItemOptions>
                    {
                        new SessionLineItemOptions {
                            PriceData = new SessionLineItemPriceDataOptions
                            {
                                Currency = "usd",
                                UnitAmount = (long)(flight.Price * req.numberOfTickets * 100),
                                ProductData = new SessionLineItemPriceDataProductDataOptions
                                {
                                    Name = flight.FlightNumber,
                                    Description = $"From: {flight.FromAirport} To: {flight.ToAirport}"
                                }
                            },
                            Quantity = 1
                            }
                    },
                    Mode="payment",
                    // ... other options ...
                    SuccessUrl=$"{HTTPReq}/api/FlightBooking/FlightSuccess/{payment.Id}",
                    CancelUrl=$"{HTTPReq}/api/FlightBooking/Cancel",
                };
                var service = new SessionService();
                Session session = await service.CreateAsync(options); // Use Async
                payment.PaymentId=session.Id;
                _payment.Update(payment);
                await unitOfWork.SaveChangesAsync();
                return GenericResponse<PaymentRes>.SuccessResponse(new PaymentRes
                {
                    Message="Payment initiated successfully",
                    PaymentId=payment.Id.ToString(),
                    Url=session.Url
                });
            }
            return GenericResponse<PaymentRes>.FailureResponse(" payment methoud is not supported for flight bookings");
        }
        public async Task<GenericResponse<PaymentRes>> HandelCancelAsync ()
        {
            return GenericResponse<PaymentRes>.FailureResponse("Payment cancelled by user");
        }

        public async Task<GenericResponse<PaymentRes>> HandelCartSuccessAsync (int orderId)
        {
            var order = await unitOfWork.OrderRepository.GetById(orderId);
            if (order==null)
            {
                return GenericResponse<PaymentRes>.FailureResponse("Order not found");
            }
            if (order.PaymentType==Domain.Models.Enums.PaymentType.Card||order.PaymentType==Domain.Models.Enums.PaymentType.Wallet)
            {
                order.Status=Domain.Models.Enums.OrderStatus.Confirmed;
                var orderItem = new List<OrderItem>();
                var cart = await _cartRepository.GetCartByUserId(order.UserId);
                if (cart==null||!cart.CartItems.Any())
                {
                    return GenericResponse<PaymentRes>.FailureResponse("Cart is empty");
                }
                foreach (var item in cart.CartItems)
                {
                    OrderItem orderItems = new OrderItem
                    {
                        OrderId=order.Id,
                        MenuItemId=item.ProductId,
                        Quantity=item.Quantity,
                        Price=item.Product.Price
                    };
                    orderItem.Add(orderItems);
                }
                await unitOfWork.OrderItems.AddOrderItemsRangeAsync(orderItem);
                unitOfWork.OrderRepository.Update(order);
                await unitOfWork.SaveChangesAsync();
                await unitOfWork.Cart.ClearCart(order.UserId);
                return GenericResponse<PaymentRes>.SuccessResponse(new PaymentRes
                {
                    Message="Payment successful"
                });

            }
            return order.PaymentType==Domain.Models.Enums.PaymentType.Cash
                ? GenericResponse<PaymentRes>.FailureResponse("Cash payment Placed Successfully")
                : GenericResponse<PaymentRes>.FailureResponse("Invalid payment type");
        }

        public async Task<GenericResponse<PaymentRes>> HandleSuccessAsync (int paymentId)
        {
            var payment = await _payment.GetById(paymentId);
            if (payment==null)
            {
                return GenericResponse<PaymentRes>.FailureResponse("Payment not found");
            }
            if (payment.PaymentFor==PaymentFor.Flight)
            {
                var flight = await unitOfWork.Flights.GetById((int)payment.FlightId, q => q.Include(f => f.Aircraft));
                if (flight==null||flight.Aircraft==null)
                    return GenericResponse<PaymentRes>.FailureResponse("Flight or Aircraft not found");
                if (flight.Aircraft.CountOfSeats<payment.NumberOfTickets)
                    return GenericResponse<PaymentRes>.FailureResponse("Not enough seats available");
                flight.Aircraft.CountOfSeats-=(int)payment.NumberOfTickets;
                payment.Status=PaymentStatus.Completed;
                var flightBooking = new FlightBooking
                {
                    FlightId=(int)payment.FlightId,
                    UserId=payment.UserId,
                    NumberOfPassengers=(int)payment.NumberOfTickets,
                    BookingDate=DateTime.UtcNow,
                    Status=FlightBookingStatus.Confirmed
                };
                await unitOfWork.FlightBookings.Add(flightBooking);
                _payment.Update(payment);
                await unitOfWork.SaveChangesAsync();
                return GenericResponse<PaymentRes>.SuccessResponse(new PaymentRes { Message="Flight booking confirmed!" });
            }
            if (payment.PaymentFor==PaymentFor.Room)
            {
                var roomBooking = new RoomBooking
                {
                    RoomId=(int)payment.RoomId,
                    UserId=payment.UserId,
                    CheckInDate=(DateTime)payment.CheckInDate,
                    CheckOutDate=(DateTime)payment.CheckOutDate
                };
                await _roomBooking.Add(roomBooking);
                _payment.Update(payment);
                await unitOfWork.SaveChangesAsync();
                return GenericResponse<PaymentRes>.SuccessResponse(new PaymentRes
                {
                    Message="Payment successful"
                });
            }
            return GenericResponse<PaymentRes>.FailureResponse("Invalid payment for type");
        }


    }
}
