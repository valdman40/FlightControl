using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightControlWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FlightControlWeb.Types;

namespace FlightControlWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightPlanController : ControllerBase
    {
        private readonly IFlightPlanManager fpManager;
        public FlightPlanController(IFlightPlanManager fpm)
        {
            fpManager = fpm;
        }

        // GET: api/FlightPlan/5
        [HttpGet("{id}")]
        public ActionResult<FlightPlan> Get(string id)
        {
            try
            {
                return Ok(fpManager.getFlightPlan(id).Result);
            }
            catch (NullReferenceException e)
            {
                return BadRequest("no such FlightPlan");
            }
        }

        // POST: api/FlightPlan 
        [HttpPost]
         public FlightPlan Post([FromBody] FlightPlan flightPlan)
        {
           return fpManager.addFlightPlan(flightPlan);
        }
    }
}
