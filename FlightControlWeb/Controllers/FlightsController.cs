using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightControlWeb.Models;
using FlightControlWeb.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightControlWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private readonly IFlightManager fManager;

        public FlightsController(IFlightManager fm)
        {
            fManager = fm;
        }

        // GET: api/Flights
        // return all flights (if sync_all is part of the args, return include external)
        [HttpGet]
        public ActionResult<List<Flight>> GetAllFlights()
        {
            if (Request.Query.ContainsKey("sync_all"))
              {
             return fManager.GetAllFlights(Request.Query["relative_to"]).Result; // date located at Request.Query["relative_to"]
              }
            return fManager.GetInternalFlights(Request.Query["relative_to"]);
        }

        // DELETE: api/Flights/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            fManager.deleteFlight(id);
        }
    }
}
