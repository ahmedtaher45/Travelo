using Microsoft.EntityFrameworkCore;
using Travelo.Domain.Shared;

namespace Travelo.Domain.Models.Entities
{
    public class Payment : BaseEntity
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        [Precision(18, 2)]
        public decimal Amount { get; set; }
        public int HotelId { get; set; }
        public Hotel Hotel { get; set; }
        public int RoomId { get; set; }
        public Room Room { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
        public string? PaymentId { get; set; }
        public PaymentStatus Status { get; set; }
    }
}
public enum PaymentStatus
{
    Pending,
    Completed,
    Failed,
    Refunded
}
