using FMS_NouraAlmaskari_.Models;
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
                Console.WriteLine("there are no Scheduled flight int the system!");
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
            if (context.Passengers.Count == 0)
            {
                Console.WriteLine("No passengers registered. please register a passenger first.");
                return;
            }

            int bookingId = context.Bookings.Count() + 1;

            Console.WriteLine("Enter passenger ID:");
            int passengerId = int.Parse(Console.ReadLine());

            var passenger = context.Bookings.FirstOrDefault(P => P.passengerId == passengerId);

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

            if (selectedFlight == null || selectedFlight.status != "Scheduled")
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
            if (context.Bookings.Count() == 0)
            {
                Console.WriteLine("No Bookings yet!");
                return;
            }

            var confirmedBookings = context.Bookings.Where(b => b.status == "Confirmed").ToList();
            if (confirmedBookings.Count() == 0)
            {
                Console.WriteLine("No confirmed bookings to cancel!");
                return;
            }

            Console.WriteLine("Enter booking ID:");
            int bookingId= int.Parse(Console.ReadLine());

            Booking booking = context.Bookings.FirstOrDefault(b=> b.bookingId == bookingId);

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

            booking.status = "Cancelled";

            Flight flight = context.Flights.FirstOrDefault(f=> f.flightId == bookingId);
            if (flight != null && flight.status== "Scheduled")
            {
                flight.availableSeats++;
            }
            else if (flight != null && flight.status != "Scheduled")
            {
                Console.WriteLine("Seats can not be returned. flight has already departed or been cancelled!");
            }


            Console.WriteLine($"Booking: {bookingId}. Cancelled successfully!");

        }
        public static void DepartFlight()
        {

        }
        public static void CancelFlight()
        {

        }
        public static void PassengerBookingHistory()
        {

        }
        public static void FlightRevenue_LoadFactorReport()
        {

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

                Console.WriteLine("Chose An Option:");
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
                        Console.WriteLine("Good Bay.");
                        break;

                    default:
                        Console.WriteLine("In valid option plz chose a valid option!");
                        break;
                }

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }
}
