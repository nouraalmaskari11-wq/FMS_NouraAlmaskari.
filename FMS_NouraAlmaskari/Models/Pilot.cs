using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS_NouraAlmaskari_.Models
{
    public class Pilot
    {
        public int pilotId { get; set; } // Unique identifier for each pilot
        public string pilotName { get; set; } // Full name of the pilot
        public string pilotPhone { get; set; } // Contact number
        public string licenseNumber { get; set; } //Official aviation license number
        public int flightHours { get; set; } // Total logged flight hours — updated after each completed flight
        public bool isAvailable { get; set; } // Indicates whether the pilot is currently free to be assigned

        public Pilot(int pilotId, string pilotName, string pilotPhone, string licenseNumber, int flightHours, bool isAvailable)
        {
            this.pilotId = pilotId;
            this.pilotName = pilotName;
            this.pilotPhone = pilotPhone;
            this.licenseNumber=licenseNumber;
            this.flightHours = flightHours;
            this.isAvailable = isAvailable;


        }

        public void printInfo()
        {
            Console.WriteLine($"Pilot ID: {pilotId} | Pilot Name: {pilotName} " +
                              $"Pilot Phone: {pilotPhone} | License Number: {licenseNumber} " +
                              $"Flight Hours: {flightHours} | Is Available: {isAvailable}");
        }
    }
}
