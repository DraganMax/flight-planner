using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using flight_planner.Models;
using WebGrease.Css.Extensions;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using RouteAttribute = System.Web.Http.RouteAttribute;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;

namespace flight_planner.Controllers
{
    public class CustomerFlightApiController : ApiController
    {
        [HttpGet]
        [Route("api/FlightSearchRequest/{id}")]
        public HttpResponseMessage Get(HttpRequestMessage request, int id)
        {
            Flight flight = FlightStorage.GetFlightById(id);
            if (flight == null)
            {
                request.CreateResponse(HttpStatusCode.NotFound);
            }
            return request.CreateResponse(HttpStatusCode.OK, flight);
        }
        [HttpGet]
        [Route("api/flights/{id}")]
        public HttpResponseMessage SearchFlightById(HttpRequestMessage request, int id)
        {
            Flight flight = FlightStorage.GetFlightById(id);
            if (flight == null)
            {
                return request.CreateResponse(HttpStatusCode.NotFound);
            }
            return request.CreateResponse(HttpStatusCode.OK, flight);
        }

        // GET: api/ClientApi/5
        [HttpGet]
        [Route("api/airports")]
        public AirportRequest[] SearchAirportsByPhrase(string search)
        {
            var flight = FlightStorage.GetFlights();
            var result = new HashSet<AirportRequest>();
            flight.ForEach(f =>
            {
                result.Add(f.From);
                result.Add(f.To);
            });

            return result.Where(a => a.Airport.ToLower().Contains(search.ToLower().Trim()) ||
                                     a.City.ToLower().Contains(search.ToLower().Trim()) ||
                                     a.Country.ToLower().Contains(search.ToLower().Trim()))
                                    .ToArray();

        }

        // POST: api/ClientApi
        [HttpPost]
        [Route("api/flights/search")]
        public HttpResponseMessage FlightSearch(HttpRequestMessage request, FlightSearchRequest search)
        {
            if (IsValid(search) && IsDifferentAirport(search))
            {
                var result = FlightStorage.GetFlights();
                var matchedItems = result.Where(f => f.From.Airport.ToLower().Contains(search.From.ToLower()) ||
                                                     f.To.Airport.ToLower().Contains(search.To.ToLower()) ||
                                                     f.DepartureTime.ToLower().Contains(search.To.ToLower())).ToList();
                var response = new FlightSearchResult
                {
                    TotalItems = result.Length,
                    Items = matchedItems,
                    Page = matchedItems.Any() ? 1 : 0
                };
                return request.CreateResponse(HttpStatusCode.OK, response);
            }
            return request.CreateResponse(HttpStatusCode.BadRequest);
        }
        private bool IsDifferentAirport(FlightSearchRequest search)
        {
            return !string.Equals(search.From, search.To, StringComparison.InvariantCultureIgnoreCase);
        }
        private bool IsValid(FlightSearchRequest search)
        {
            return search != null && !string.IsNullOrEmpty(search.From) &&
                !string.IsNullOrEmpty(search.To) &&
                !string.IsNullOrEmpty(search.DepartureDate);
        }
    }
}