using Microsoft.EntityFrameworkCore;
using Travelo.Application.Common.Responses;
using Travelo.Application.DTOs.Booking;
using Travelo.Application.DTOs.Payment;
using Travelo.Application.Interfaces;
using Travelo.Domain.Models.Entities;
using Travelo.Domain.Models.Enums;

namespace Travelo.Application.Services.Booking
{
    public class BookingService : IBookingService
    {
        private readonly IGeneralBookingRepository _bookingRepository;
        private readonly IFlightRepository _flightRepository;
        private readonly IUnitOfWork unitOfWork;

        public BookingService (
            IGeneralBookingRepository bookingRepository,
            IFlightRepository flightRepository,
            IUnitOfWork unitOfWork)
        {
            _bookingRepository=bookingRepository;
            _flightRepository=flightRepository;
            this.unitOfWork=unitOfWork;
        }
        public async Task<GenericResponse<IEnumerable<BookingDetailsDto>>> GetAllFlightBookingAsync ()
        {
            var flightBookings = await unitOfWork.GeneralBooking
                .GetManyAsync(b => b.Type==BookingType.Flight, include: q => q.Include(b => b.Flight).Include(b => b.User));
            if (flightBookings==null||!flightBookings.Any())
                return GenericResponse<IEnumerable<BookingDetailsDto>>.FailureResponse("No flight bookings found");

            var flightBookingDtos = flightBookings.Select(b => new BookingDetailsDto
            {
                Id=b.Id,
                UserId=b.User.Id,
                UserNamw=b.User.UserName,
                Name=b.Flight!=null ? b.Flight.FlightNumber : $"Flight #{b.FlightId}",
                Address=b.Flight!=null ? $"{b.Flight.FromAirport} → {b.Flight.ToAirport}" : "",
                Type=b.Type,
                FromDate=b.FromDate,
                ToDate=b.ToDate,
                BasePrice=b.TotalPrice,
                Taxes=b.PriceDetails?.Taxes??0,
                Total=b.TotalPrice,
                Status=b.Status
            }).ToList();

            return GenericResponse<IEnumerable<BookingDetailsDto>>.SuccessResponse(flightBookingDtos);
        }
        public async Task<GenericResponse<IEnumerable<RoomBookingPaymentRes>>> GetAllRoomBookingAsync ()
        {
            var roomboking = await unitOfWork.GeneralBooking.GetManyAsync(e => e.Type==BookingType.Room, include: q => q.Include(b => b.Hotel).Include(b => b.Room).Include(b => b.User));
            if (roomboking==null||!roomboking.Any())
                return GenericResponse<IEnumerable<RoomBookingPaymentRes>>.FailureResponse("No Room bookings found");
            var roomsBooking = roomboking.Select(b => new RoomBookingPaymentRes
            {
                id=b.Id,
                UserId=b.UserId,
                UserName=b.User.UserName,
                HotelId=(int)b.HotelId,
                HotelName=b.Hotel.Name,
                RoomId=(int)b.RoomId,
                CheckInDate=b.FromDate,
                CheckOutDate=b.ToDate
            });
            return GenericResponse<IEnumerable<RoomBookingPaymentRes>>.SuccessResponse(roomsBooking);
        }

        public async Task<GenericResponse<IEnumerable<RoomBookingPaymentRes>>> GetHotelRoomBookingAsync (int hotilId)
        {
            var hotelBookingRoom = await unitOfWork.GeneralBooking.GetManyAsync(e => e.HotelId==hotilId&&e.Type==BookingType.Room, include: q => q.Include(b => b.Hotel).Include(b => b.Room).Include(b => b.User));
            if (hotelBookingRoom==null||!hotelBookingRoom.Any()) return GenericResponse<IEnumerable<RoomBookingPaymentRes>>.FailureResponse($"No Room Booking for this Hotel {hotilId} ");
            var lestOfRoomBooking = hotelBookingRoom.Select(b => new RoomBookingPaymentRes
            {
                id=b.Id,
                UserId=b.UserId,
                UserName=b.User.UserName,
                HotelId=(int)b.HotelId,
                HotelName=b.Hotel.Name,
                RoomId=(int)b.RoomId,
                CheckInDate=b.FromDate,
                CheckOutDate=b.ToDate
            });
            return GenericResponse<IEnumerable<RoomBookingPaymentRes>>.SuccessResponse(lestOfRoomBooking);
        }
        public async Task<BookingDto> CreateFlightBookingAsync (CreateBookingDto dto)
        {
            var flight = await _flightRepository.GetByIdAsync(dto.FlightId);
            if (flight==null)
                throw new Exception("Flight not found");

            var booking = new Travelo.Domain.Models.Entities.GeneralBooking
            {
                FlightId=flight.Id,
                Status=BookingStatus.Pending,
                TotalPrice=flight.Price,
                FromDate=flight.ArrivalDateTime,
                ToDate=flight.ArrivalDateTime,
                //UserId = dto.
            };

            //await _bookingRepository.Add(booking);
            //await _bookingRepository.SaveChangesAsync();

            return MapToDto(booking);
        }

        public async Task<BookingDto> ConfirmBookingAsync (int bookingId)
        {
            var booking = await _bookingRepository.GetById(bookingId);
            if (booking==null)
                throw new Exception("Booking not found");

            if (booking.Status==BookingStatus.Cancelled)
                throw new Exception("Booking is cancelled");

            if (booking.Status!=BookingStatus.Pending)
                throw new Exception("Booking cannot be confirmed");


            booking.Status=BookingStatus.Confirmed;

            await unitOfWork.SaveChangesAsync();

            return MapToDto(booking);
        }

        public async Task CancelBookingAsync (int bookingId)
        {
            var booking = await _bookingRepository.GetById(bookingId);
            if (booking==null)
                throw new Exception("Booking not found");

            booking.Status=BookingStatus.Cancelled;
            await unitOfWork.SaveChangesAsync();
        }

        private BookingDto MapToDto (GeneralBooking b)
        {
            return new BookingDto
            {
                Id=b.Id,
                FlightId=(int)b.FlightId,
                Status=b.Status,
                TotalPrice=b.TotalPrice,
                TicketId=b.Id
            };
        }

        public async Task<TripDto> CreateAsync (
            string userId,
            CreateGeneralBookingDto dto)
        {
            var booking = new GeneralBooking
            {
                UserId=userId,
                Type=dto.Type,

                FlightId=dto.FlightId,
                HotelId=dto.HotelId,
                RoomId=dto.RoomId,

                FromDate=dto.FromDate,
                ToDate=dto.ToDate,

                Status=BookingStatus.Pending,
                TotalPrice=0
            };

            await _bookingRepository.Add(booking);
            await unitOfWork.SaveChangesAsync();

            return MapToTripDto(booking);
        }

        public async Task<List<TripDto>> GetMyTripsAsync (string userId)
        {
            var bookings =
                await _bookingRepository.GetManyAsync(e => e.UserId==userId);

            return bookings
                .Select(b => MapToTripDto(b))
                .ToList();
        }


        public async Task<BookingDetailsDto?> GetDetailsAsync (int id)
        {
            var booking =
                await _bookingRepository.GetById(id);

            return booking==null ? null : MapToDetailsDto(booking);
        }


        public async Task CancelAsync (int id)
        {
            var booking =
                await _bookingRepository.GetById(id);

            if (booking==null)
                throw new Exception("Not found");

            booking.Status=BookingStatus.Cancelled;

            _bookingRepository.Update(booking);
            await unitOfWork.SaveChangesAsync();
        }

        // ===================== Mappers =====================

        private TripDto MapToTripDto (GeneralBooking b)
        {
            return new TripDto
            {
                Id=b.Id,

                Title=b.Type switch
                {
                    BookingType.Flight => b.Flight?.FromAirport+" → "+b.Flight?.ToAirport,
                    BookingType.Hotel => b.Hotel?.Name,
                    BookingType.Room => b.Room?.View,
                    _ => "Trip"
                },

                Type=b.Type,

                From=b.FromDate,
                To=b.ToDate,

                Status=b.Status,
                Price=b.TotalPrice
            };
        }

        private BookingDetailsDto MapToDetailsDto (GeneralBooking b)
        {
            return new BookingDetailsDto
            {
                Id=b.Id,

                Name=b.Type switch
                {
                    BookingType.Flight => b.Flight?.FlightNumber,
                    BookingType.Hotel => b.Hotel?.Name,
                    BookingType.Room => b.Room?.View,
                    _ => ""
                },

                Type=b.Type,

                FromDate=b.FromDate,
                ToDate=b.ToDate,

                BasePrice=b.PriceDetails?.BaseFare??0,
                Taxes=b.PriceDetails?.Taxes??0,
                Total=b.TotalPrice,

                Status=b.Status
            };
        }

    }
}
