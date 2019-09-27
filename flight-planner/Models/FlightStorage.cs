using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace flight_planner.Models
{
    public class FlightStorage
    {
        private static int _id = 1;
        private static readonly object obj = new object();
        public static SynchronizedCollection<Flight> _flights { get; set; }
        static FlightStorage()
        {
            _flights = new SynchronizedCollection<Flight>();
        }

        public static Flight[] GetFlights()
        {
            return _flights.ToArray();
        }

        public static void RemoveFlight(Flight flight)
        {
            _flights.Remove(flight);
        }

        public static void ClearList()
        {
            _flights.Clear();
        }

        public static bool AddFlight(Flight flight)
        {
            lock (obj)
            {
                if (!_flights.Any(f => f.Equals(flight)))
                {
                    _flights.Add(flight);
                    return true;
                }
                return false;
            }

        }
        public static void RemoveFlightById(int id)
        {
            lock (obj)
            {
                var flight = GetFlightById(id);
                if (flight != null)
                {
                    _flights.Remove(flight);
                }
            }
        }
        public static Flight GetFlightById(int id)
        {
            var pair = _flights.FirstOrDefault(f => f.Id == id);
            return pair;
        }

        public static int GetId()
        {
            return _id++;
        }
    }
}