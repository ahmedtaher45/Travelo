using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travelo.Domain.Models.Entities
{
    internal class Hotel : IdentityUser
    {
        public string ResponsibleName { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        //public int CityId { get; set; }
        //public City City { get; set; }

    }
}
