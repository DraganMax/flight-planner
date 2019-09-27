using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Routing;
using flight_planner.Attributes;
using flight_planner.Models;

namespace flight_planner.Controllers
{
    [BasicAuthentication]
    public class AdminApiController : ApiController
    {
        // GET: api/AdminApi
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/AdminApi/5
        [HttpGet]
        [Route("admin-api/flights/{id}")]
        public HttpResponseMessage Get(HttpRequestMessage request, int id)
        {
            var flight = FlightStorage.GetFlightById(id);
            if (flight == null)
            {
                return request.CreateResponse(HttpStatusCode.NotFound);
            }
            return request.CreateResponse(HttpStatusCode.OK, flight);
        }

        // POST: api/AdminApi
        public void Post([FromBody] string value)
        {
        }

        private static bool ValidateDates(string departure, string arrival)
        {
            if (!string.IsNullOrEmpty(departure) && !string.IsNullOrEmpty(arrival))
            {
                var departureDate = DateTime.Parse(departure);
                var arrivalDate = DateTime.Parse(arrival);
                return DateTime.Compare(arrivalDate, departureDate) > 0;
            }

            return false;
        }

        private bool IsGoodAirports(AirportRequest to, AirportRequest from)
        {
            return to.City.ToUpper() != from.City.ToUpper() &&
                   to.Airport.ToUpper() != from.Airport.ToUpper();
                   //to.Country.ToUpper() != from.Country.ToUpper();
        }

        private bool IsValidAirport(AirportRequest airport)
        {
            return !string.IsNullOrEmpty(airport.Airport) &&
                   !string.IsNullOrEmpty(airport.City) &&
                   !string.IsNullOrEmpty(airport.Country);
        }

        private bool IsValid(Flight flight)
        {
            return !string.IsNullOrEmpty(flight.ArrivalTime) &&
                   !string.IsNullOrEmpty(flight.Carrier) &&
                   !string.IsNullOrEmpty(flight.DepartureTime) &&
                   flight.To != null &&
                   flight.From != null &&
                   IsValidAirport(flight.To) &&
                   IsValidAirport(flight.From) &&
                   ValidateDates(flight.DepartureTime, flight.ArrivalTime) &&
                   IsGoodAirports(flight.To, flight.From);
        }

        // PUT: api/AdminApi/5
        [HttpPut]
        [Route("admin-api/flights")]
        public HttpResponseMessage AddFlight(HttpRequestMessage request, Flight flight)
        {
            if (!IsValid(flight))
            {
                return request.CreateResponse(HttpStatusCode.BadRequest);
            }

            flight.Id = FlightStorage.GetId();

            if (!FlightStorage.AddFlight(flight))
            {
                return request.CreateResponse(HttpStatusCode.Conflict, flight);
            }

            return request.CreateResponse(HttpStatusCode.Created, flight);

        }

        // DELETE: api/AdminApi/5
        [HttpDelete]
        [Route("admin-api/flights/{id}")]
        public HttpResponseMessage Delete(HttpRequestMessage request, int id)
        {
            FlightStorage.RemoveFlightById(id);

            return request.CreateResponse(HttpStatusCode.OK);
        }
    }
}