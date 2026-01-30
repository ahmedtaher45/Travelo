using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travelo.Application.DTOs.Booking;
using Travelo.Domain.Models.Entities;

namespace Travelo.Application.Services.Booking
{
    public interface IBookingService
    {
        Task<BookingDto> CreateFlightBookingAsync(CreateBookingDto dto);
        Task<BookingDto> ConfirmBookingAsync(int bookingId);
        Task CancelBookingAsync(int bookingId);
    }

}
