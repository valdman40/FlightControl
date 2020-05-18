using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightControlWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace FlightControlWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightController : ControllerBase
    {
        private readonly SqliteDataAccess data = new SqliteDataAccess();
        // GET: api/Flight
        [HttpGet]
        public IEnumerable<Flight> GetAllFlights()
        {
            // return data.GetAllFlights();
            return SqliteDataAccess.LoadFlights();
        }

        /*
        // GET: api/Flight/5
        [HttpGet("{id}")]
        public Flight Get(int id)
        {
           return data.GetFlightById(id);
        }

        */
        // POST: api/Flight
        [HttpPost]
        public Flight Post(Flight f)
        {
            SqliteDataAccess.SaveFlight(f);
            return f;
        }

        // PUT: api/Flight/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
