using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travelo.Application.DTOs.Hotels
{
    public class HotelPolicyDto
    {
        public string CheckIn { get; set; } = "From 15:00 to 18:00";
        public string CheckOut { get; set; } = "From 8:00 to 11:00";
        public string Cancellation { get; set; } = "Cancellation and prepayment policies vary according to accommodation type.";
        public string ChildrenAndBeds { get; set; } = "Children of any age are welcome. Cots and extra beds are not available.";
        public string AgeRestriction { get; set; } = "Guests of all ages are welcome.";
        public string QuietHours { get; set; } = "Guests must be quiet between 22:00 and 10:00.";
        public string Smoking { get; set; } = "Smoking is not allowed.";
        public string Pets { get; set; } = "Pets are not allowed.";
    }
}
