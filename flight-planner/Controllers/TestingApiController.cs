using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using flight_planner.Models;

namespace flight_planner.Controllers
{
    public class TestingApiController : ApiController
    {
        [HttpPost]
        [Route("testing-api/clear")]
        public bool Clear()
        {
            FlightStorage.ClearList();
            return true;
        }

        public string Get()
        {
            return "Testing api";
        }
    }
}