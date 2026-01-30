using System.Text.Json.Serialization;
using Travelo.Domain.Models.Enums;

namespace Travelo.Application.DTOs.Payment
{
    public class FlightPaymentRequest
    {
        public int FlightId { get; set; }
        public int numberOfTickets { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PaymentType PaymentType { get; set; }
    }
}
