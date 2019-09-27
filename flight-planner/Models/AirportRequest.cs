using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;

namespace flight_planner.Models
{
    public class AirportRequest
    {
        public string Country { get; set; }
        public string City { get; set; }
        public string Airport { get; set; }

        public override bool Equals(object obj)
        {
            var airport = obj as AirportRequest;
            if (airport == null)
            {
                return false;
            }
            return airport.City == City && airport.Airport == Airport && airport.Country == Country;
        }
    }
}