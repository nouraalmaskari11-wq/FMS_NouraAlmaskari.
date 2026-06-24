using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS_NouraAlmaskari_.Models
{
    public class Booking
    {
        public int bookingId { get; set; } //Unique identifier for each booking
        public int passengerId { get; set; } //ID of the passenger who made the booking
        public int flightId { get; set; } //ID of the flight being booked
        public string seatNumber { get; set; } // Seat label assigned at booking (e.g. 14A)
        public string bookingDate { get; set; } //Date the booking was made
        public decimal totalPrice { get; set; } //Price paid — taken from the flight's ticket price at the time of booking
        public string status { get; set; } // Confirmed | Cancelled

        public Booking(int bookingId, int passengerId, int flightId, string seatNumber, string bookingDate, decimal totalPrice, string status)
        {
            this.bookingId = bookingId;
            this.passengerId = passengerId;
            this.flightId = flightId;
            this.seatNumber = seatNumber;
            this.bookingDate = bookingDate;
            this.totalPrice = totalPrice;
            this.status = status; 
        }

        public void printInfo()
        {
            Console.WriteLine($"Booking ID: {bookingId} | Passenger ID: {passengerId} | Flight ID: {flightId}" +
                $"Seat Number :{seatNumber} | Booking Date: {bookingDate} | Total Price: {totalPrice}" +
                $"Status: {status}");
        }
    }
}
