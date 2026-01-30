using System.Text.Json.Serialization;
using Travelo.Domain.Models.Enums;

namespace Travelo.Application.DTOs.Payment
{
    public class CheckoutReq
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PaymentType PaymentType { get; set; }
    }
}
