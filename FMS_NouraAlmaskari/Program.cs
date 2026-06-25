using FMS_NouraAlmaskari_.Models;
using Microsoft.VisualBasic;
using System.Net.NetworkInformation;
using System.Numerics;
namespace FMS_NouraAlmaskari_
{
    internal class Program
    {
        public static FlightContext context = new FlightContext
        {
            Aircrafts = new List<Aircraft>(),
            Bookings = new List<Booking>(),
            Flights = new List<Flight>(),
            Passengers = new List<Passenger>(),
            Pilots = new List<Pilot>()
        };

        public static void RegisterPassenger()
        {
            int passengerId = context.Passengers.Count() + 1;

            Console.WriteLine("Enter Passenger Full Name: ");
            string passengerName = Console.ReadLine();

            Console.WriteLine("Enter Passenger Email: ");
            string passengerEmail = Console.ReadLine();

            Console.WriteLine("Enter Passenger Phone: ");
            string passengerPhone = Console.ReadLine();

            Console.WriteLine("Enter Passenger passport number");
            string passportNumber = Console.ReadLine();

            if (context.Passengers.Any(p => p.passportNumber == passportNumber))
            {
                Console.WriteLine("Error: Passport number already exists!");
                return;
            }

            Console.WriteLine("Enter Passenger Nationality");
            string nationality = Console.ReadLine();

            context.Passengers.Add(new Passenger ( passengerId, passengerName, passengerEmail , passengerPhone , passportNumber, nationality ));

            Console.WriteLine($"Passenger registered successfully. Assigned ID: {passengerId}");
        }
        
        public static void AddAircraft()
        {
             int aircraftId = context.Aircrafts.Count() + 1;

            Console.WriteLine("Enter Aircraft model name (e.g. Boeing 737, Airbus A320)");
            string model= Console.ReadLine();

            Console.WriteLine("Enter Total number of passenger seats on this aircraft");
            int totalSeats = int.Parse(Console.ReadLine()); 

           context.Aircrafts.Add(new Aircraft(aircraftId,model, totalSeats,true));

            Console.WriteLine($"Aircraft Added successfully. Assigned ID: {aircraftId}");


        }

        public static void RegisterPilot()
        {
            int pilotId = context.Pilots.Count() + 1;

            Console.WriteLine("Enter Pilot Full Name:");
            string pilotName = Console.ReadLine();

            Console.WriteLine("Enter Pilot Contact number:");
            string pilotPhone = Console.ReadLine();

            Console.WriteLine("Enter Pilot Official aviation license number:");
            string licenseNumber = Console.ReadLine();
            if (context.Pilots.Any(p => p.licenseNumber == licenseNumber))
            {
                Console.WriteLine("Error: License number already exists!");
                return;
            }

            Console.WriteLine("Enter Pilot Total logged flight hours:");
            int flightHours = int.Parse(Console.ReadLine());
           
            context.Pilots.Add(new Pilot(pilotId, pilotName, pilotPhone, licenseNumber, flightHours, true));

            Console.WriteLine($" Pilot Added successfully. Assigned ID: {pilotId}");
        }

        public static void ViewAllFlights()
        {
            if (context.Flights.Count == 0)
            {
                Console.WriteLine("there are no Scheduled flight in the system!");
                return;
            }

            Console.WriteLine(" --- All flight ---");
            Console.WriteLine("---------------------");
            foreach(var flight in context.Flights)
            {
                flight.printInfo();
            }
        }

        public static void ScheduleFlight()
        {
            var available = context.Aircrafts.Where(a => a.isOperational).ToList();
            if (available.Count == 0)
            {
                Console.WriteLine("No Operational Aircrafts is available!");
                return;
            }

            Console.WriteLine(" Available Aircrafts:");
                foreach (var aircraft in available) 
            { 
                    aircraft.printInfo();
            }

            Console.WriteLine("Enter Aircraft ID: ");
            int aircraftId =int.Parse(Console.ReadLine());

            Aircraft selectedAircraft = context.Aircrafts.FirstOrDefault(s=> s.aircraftId == aircraftId);

            if (selectedAircraft == null || !selectedAircraft.isOperational)
            {
                Console.WriteLine("Invalid Aircraft Id or Aircraft not operational!");
                return;
            }


            List<Pilot> pilot= context.Pilots.Where (p=> p.isAvailable).ToList();

            if (pilot.Count == 0)
            {
                Console.WriteLine("No pilots is available");
                return;
            }
            Console.WriteLine("Available pilots :");
            foreach (Pilot pilot1 in pilot) 
            {
                pilot1.printInfo();
            }

            Console.WriteLine(" Enter pilot Id: ");
            int pilotId =int.Parse(Console.ReadLine());

            Pilot selectedPilot = context.Pilots.FirstOrDefault(p=> p.pilotId == pilotId);
            if (selectedPilot == null || !selectedPilot.isAvailable)
            {
                Console.WriteLine("Invalid pilot Id or Pilot not available!");
                return;
            }

            int flightId = context.Flights.Count()+1;

            string flightCode = $"OA-{flightId:D3}";

            Console.WriteLine("Enter the Departure airport / city");
            string origin= Console.ReadLine();

            Console.WriteLine("Enter the Arrival airport / city");
            string destination = Console.ReadLine();

            Console.WriteLine("Enter departure date");
            string departureDate = Console.ReadLine();

            Console.WriteLine("Enter departure time");
            string departureTime = Console.ReadLine();

            Console.WriteLine("Enter duration of the in minutes:");
            int duration= int.Parse(Console.ReadLine());

            Console.WriteLine("Enter Price per seat for this flight:");
            decimal ticketPrice= decimal.Parse(Console.ReadLine());

            context.Flights.Add(new Flight(flightId, flightCode, aircraftId, pilotId, origin, destination, departureDate, departureTime, duration, ticketPrice, selectedAircraft.totalSeats, "Scheduled"));

            Console.WriteLine($"Flight scheduled successfully! Flight Code: {flightCode}, Flight ID: {flightId}");
            Console.WriteLine($"Duration: {duration} minutes");

            selectedPilot.isAvailable = false;
        }


        public static void BookFlight()
        {
            if (!context.Passengers.Any())
            {
                Console.WriteLine("No passengers registered. please register a passenger first.");
                return;
            }

            int bookingId = context.Bookings.Count() + 1;

            Console.WriteLine("Enter passenger ID:");
            int passengerId = int.Parse(Console.ReadLine());

            var passenger = context.Passengers.FirstOrDefault(P => P.passengerId == passengerId);

            if (passenger == null)
            {
                Console.WriteLine("Passenger not found!");
                return;
            }

            List<Flight> availableFlights = context.Flights.Where(f => f.status == "Scheduled" && f.availableSeats > 0).ToList();
            if (availableFlights.Count == 0)
            {
                Console.WriteLine("No available flight with seats!");
                return;
            }


            foreach (Flight f in availableFlights)
            {
                f.printInfo();
            }

            Console.WriteLine("Enter Flight ID:");
            int flightId = int.Parse(Console.ReadLine());

            var selectedFlight = context.Flights.FirstOrDefault(f => f.flightId == flightId);

            if (selectedFlight == null || selectedFlight.status != "Scheduled" || selectedFlight.availableSeats <= 0)
            {
                Console.WriteLine("Invalid flight or no seats available!");
                return;
            }

            string seatNumber = $"{selectedFlight.availableSeats}A";
            string bookingDate = DateTime.Now.ToString("yyyy-MM-dd");

            selectedFlight.availableSeats--;

            context.Bookings.Add(new Booking(bookingId, passengerId, flightId, seatNumber, bookingDate, selectedFlight.ticketPrice, "Confirmed"));

            Console.WriteLine($"Booking confirmed! Booking ID: {bookingId}, seat: {seatNumber},  Price: {selectedFlight.ticketPrice}");
        }

        public static void CancelBooking()
        {
            if (!context.Bookings.Any(b=>b.status=="Confirmed"))
            {
                Console.WriteLine("No Bookings yet!");
                return;
            }

            Console.WriteLine("Enter booking ID:");
            int bookingId = int.Parse(Console.ReadLine());

            Booking booking = context.Bookings.FirstOrDefault(b => b.bookingId == bookingId);

            if (booking == null)
            {
                Console.WriteLine("Booking not found!");
                return;
            }

            if (booking.status == "Cancelled")
            {
                Console.WriteLine("The booking is already cancelled!");
                return;
            }


            Flight flight = context.Flights.FirstOrDefault(f => f.flightId == booking.flightId);

            if (flight == null)
            {
                Console.WriteLine("Associated flight not found!");
                return;
            }


            if (flight.status != "Scheduled")
            {
                Console.WriteLine("Cannot cancel booking because the flight has already departed or been cancelled.");
                return;
            }

            booking.status = "Cancelled";

            flight.availableSeats++;
                      
            Console.WriteLine($"Booking: {bookingId}. Cancelled successfully!");

        }

        public static void DepartFlight()
        {
            if (!context.Flights.Any(f=>f.status == "Scheduled"))
            {
                Console.WriteLine("No scheduled flights to depart.");
                return;
            }

            Console.WriteLine("Enter flight ID to depart: ");
            int flightId =int.Parse(Console.ReadLine());

            Flight flight = context.Flights.FirstOrDefault(f => f.flightId == flightId);
            if (flight == null)
            {
                Console.WriteLine("Flight not found.");
                return;
            }

            if (flight.status != "Scheduled")
            {
                Console.WriteLine($"Flight is already {flight.status}. can not depart");
                return;
            }

            flight.status = "Departed";

            Pilot pilot = context.Pilots.FirstOrDefault(p => p.pilotId == flight.pilotId);
            if (pilot != null)
            {
                int hours = Math.Max(1, flight.duration / 60);
                pilot.flightHours += hours;

                pilot.isAvailable = true;
                Console.WriteLine($"Pilot {pilot.pilotName} hours increased by {hours}");
            }

            Console.WriteLine($"Flight {flight.flightCode} has departed.");
        }

        public static void CancelFlight()
        {
            if (!context.Flights.Any(f => f.status == "Scheduled"))
            {
                Console.WriteLine("No scheduled flights to cancel.");
                return;
            }

            Console.WriteLine("Enter flight ID to cancel: ");
            int flightId = int.Parse(Console.ReadLine());

            Flight flight = context.Flights.FirstOrDefault(f => f.flightId == flightId);
            if (flight == null)
            {
                Console.WriteLine("Flight not found.");
                return;
            }

            if (flight.status != "Scheduled")
            {
                Console.WriteLine($"Flight is already {flight.status}. can not cancel");
                return;
            }

            List<Booking> toCancelBookings = context.Bookings.Where(b => b.flightId == flightId && b.status == "Confirmed").ToList();

           
            foreach (Booking b in toCancelBookings)
            {
                b.status = "Cancelled";
                flight.availableSeats++;
              
            }

            int affectedBookings = toCancelBookings.Count;

            flight.status = "Cancelled";

            Pilot pilot = context.Pilots.FirstOrDefault(p => p.pilotId == flight.pilotId);

            if (pilot != null)
            {
                pilot.isAvailable = true;
                Console.WriteLine($"Pilot {pilot.pilotName} now is available.");
            }

            Console.WriteLine($"Flight {flight.flightCode} cancelled. {affectedBookings} bookings were cancelled.");

        }

        public static void PassengerBookingHistory()
        {
            if (!context.Passengers.Any())
            {
                Console.WriteLine("No passenger registered.");
                return;
            }

            Console.WriteLine("Enter passenger ID: ");
            int passengerId = int.Parse(Console.ReadLine());

            Passenger passenger = context.Passengers.FirstOrDefault(p => p.passengerId == passengerId);
            if (passenger == null)
            {
                Console.WriteLine("Passenger not found.");
                return;
            }

            List<Booking> bookings = context.Bookings.Where(b => b.passengerId == passengerId).ToList();

            if (bookings.Count == 0)
            {
                Console.WriteLine($"No bookings found for {passenger.passengerName}.");
                return;
            }
            Console.WriteLine($"Booking history for {passenger.passengerName}:");
            decimal totalAmount = 0;
            foreach (Booking booking in bookings)
            {
                Flight flight = context.Flights.FirstOrDefault(f => f.flightId == booking.flightId);

                if (flight == null)
                {
                    Console.WriteLine($"Flight not found for booking {booking.bookingId}");
                    continue;
                }

                Console.WriteLine($"Flight code: {flight.flightCode} | Origin: {flight.origin} | Destination: {flight.destination}" +
                    $"Departure date: {flight.departureDate} | Seat Number: {booking.seatNumber} | Price paid: {booking.totalPrice}" +
                    $"Booking status: {booking.status}");

                if (booking.status == "Confirmed")
                {
                    totalAmount += booking.totalPrice;
                }
            }
            Console.WriteLine($"Total amount spent on confirmed bookings: {totalAmount}");
        }
        
        public static void FlightRevenue_LoadFactorReport()
        {
            if (!context.Flights.Any())
            {
                Console.WriteLine("No flight in the system.");
                return;
            }

            Console.WriteLine("Flight revenue & Load factor report");

            var report = context.Flights.Select(flight => new
            {
                Flight = flight,
                confirmedBookings = context.Bookings.Count(b => b.flightId == flight.flightId && b.status == "Confirmed"),
                TotalRevenue = context.Bookings.Where(b => b.flightId == flight.flightId && b.status == "Confirmed").Sum(b => b.totalPrice)
            }).ToList();

            var storedReport = report.OrderByDescending(r => r.TotalRevenue).ToList();
            foreach (var item in storedReport)
            {
                var flight = item.Flight;
                var aircraft = context.Aircrafts.FirstOrDefault(a => a.aircraftId == flight.aircraftId);

                double loadFactor = 0;

                if (aircraft != null && aircraft.totalSeats > 0)
                {
                    loadFactor = (double)item.confirmedBookings / aircraft.totalSeats * 100;
                }

                Console.WriteLine($"Flight Code: {flight.flightCode} | Origin: {flight.origin} | Destination: {flight.destination}" +
                    $"Confirmed: {item.confirmedBookings} | Revenue: {item.TotalRevenue:C}" +
                    $"Load Factor: {loadFactor:F1}%");
            }
            decimal grandTotal = storedReport.Sum(r => r.TotalRevenue);
            Console.WriteLine($"Grand total revenue: {grandTotal}");

        }


public static void Main(string[] args)
        {

            bool Exit = false;

            while (!Exit)
            {
                Console.WriteLine("   Flight Management System  ");
                Console.WriteLine(" ---------------------------- ");
                Console.WriteLine("   1.  Register a Passenger   ");
                Console.WriteLine("   2.  Add an Aircraft   ");
                Console.WriteLine("   3.  Register a Pilot  ");
                Console.WriteLine("   4.  View All Flights  ");
                Console.WriteLine("   5.  Schedule a Flight ");
                Console.WriteLine("   6.  Book a Flight     ");
                Console.WriteLine("   7.  Cancel a Booking  ");
                Console.WriteLine("   8.  Depart a Flight   ");
                Console.WriteLine("   9.  Cancel a Flight   ");
                Console.WriteLine("   10. Passenger Booking History ");
                Console.WriteLine("   11. Flight Revenue & Load Factor Report ");
                Console.WriteLine("   0.  Exit ");

                Console.WriteLine("Choose An Option:");
                int option = int.Parse(Console.ReadLine());

                switch (option)
                {
                    case 1:
                        RegisterPassenger();
                        break;
                    case 2:
                        AddAircraft();
                        break;
                    case 3:
                        RegisterPilot();
                        break;
                    case 4:
                        ViewAllFlights();
                        break;
                    case 5:
                        ScheduleFlight();
                        break;
                    case 6:
                        BookFlight();
                        break;
                    case 7:
                        CancelBooking();
                        break;
                    case 8:
                        DepartFlight();
                        break;
                    case 9:
                        CancelFlight();
                        break;
                    case 10:
                        PassengerBookingHistory();
                        break;
                    case 11:
                        FlightRevenue_LoadFactorReport();
                        break;
                    case 0:
                        Exit = true;
                        Console.WriteLine("Thank you for using Flight Management System.");
                        Console.WriteLine("Goodbye.");
                        break;

                    default:
                        Console.WriteLine("Invalid option plz chose a valid option!");
                        break;
                }

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }
}
