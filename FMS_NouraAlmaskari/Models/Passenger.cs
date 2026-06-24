using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS_NouraAlmaskari_.Models
{
    public class Passenger
    {
        public int passengerId { get; set; } // Unique identifier for every passenger in the system
        public string passengerName { get; set; } // Full name of the passenger
        public string passengerEmail { get; set; } // Email address used for booking confirmation
        public string passengerPhone { get; set; } // Contact phone number
        public string passportNumber { get; set; } // Passport / national ID number — must be unique per passenger
        public string nationality { get; set; } // Country of the passenger's passport

        public Passenger(int passengerId, string passengerName, string passengerEmail, string passengerPhone, string passportNumber, string nationality)
        {
            this.passengerId = passengerId;
            this.passengerName = passengerName;
            this.passengerEmail = passengerEmail;
            this.passengerPhone = passengerPhone;
            this.passportNumber = passportNumber;
            this.nationality = nationality;
        }

        public void printInfo()
        {
            Console.WriteLine($"Passenger ID: {passengerId} | Passenger Name: {passengerName} " +
                              $" Passenger Email: {passengerEmail} | Passenger Phone: {passengerPhone} " +
                              $" Passport Number: {passportNumber} | Nationality: {nationality}");
        }
    }
}
