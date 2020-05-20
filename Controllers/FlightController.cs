﻿using System;
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
        private readonly IFlightManager fManager;
        public FlightController(IFlightManager fm)
        {
            fManager = fm;
        }
        // GET: api/Flight
        [HttpGet]
        public IEnumerable<Flight> GetAllFlights()
        {
            if (Request.Query.ContainsKey("sync_all"))
            {
                return fManager.GetAllFlights(Request.Query["relative_to"]); // date located at Request.Query["relative_to"]
            }
            return fManager.GetInternalFlights(Request.Query["relative_to"]);
        }

        
        // GET: api/Flight/5
        [HttpGet("{id}")]
        public Flight Get(int id)
        {
            return fManager.getFlight(id);
        }

        // POST: api/Flight
        [HttpPost]
        public Flight Post(Flight f) // need to delete
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
            fManager.deleteFlight(id);
        }


    }
}
