using Travelo.Domain.Shared;

namespace Travelo.Domain.models.Entities
{
    public class Aircraft : BaseEntity
    {
        public string? Model { get; set; }
        public int CountOfSeats { get; set; }
    }
}
