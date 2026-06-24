using FMS_NouraAlmaskari_.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS_NouraAlmaskari_
{
    public class FlightContext
    {
        public List<Aircraft> Aircrafts { get; set; }
        public List<Booking> Bookings { get; set; }
        public List<Flight> Flights { get; set; }
        public List<Passenger> Passengers { get; set; }
        public List<Pilot> Pilots { get; set; }
       
}
}
