using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS_NouraAlmaskari_.Models
{
    public class Aircraft
    {
        public int aircraftId { get; set; } // Unique identifier for each aircraft
        public string model { get; set; } // Aircraft model name (e.g. Boeing 737, Airbus A320)
        public int totalSeats { get; set; } // Total number of passenger seats on this aircraft
        public bool isOperational { get; set; } // True if the aircraft is airworthy; false if grounded for maintenance

        public Aircraft(int aircraftId, string model, int totalSeats, bool isOperational)
        {
            this.aircraftId = aircraftId;
            this.model = model;
            this.totalSeats = totalSeats;
            this.isOperational = isOperational;
        }

        public void printInfo()
        {
            Console.WriteLine($"Aircraft ID:{aircraftId} | Model: {model} " +
                $"Total Seats: {totalSeats} | Is Operational: {isOperational}");
        }
    }
}
