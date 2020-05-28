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
    public class serversController : ControllerBase
    {
        private readonly IServerManager sManager;
        public serversController(IServerManager sm)
        {
            sManager = sm;
        }

        // GET: api/servers
        // returns all servers
        [HttpGet]
        public IEnumerable<dynamic> GetAllServers()
        {
            return sManager.getServers();
        }

        // POST: api/servers
        // info in post body
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult Post([FromBody] Server newServer)
        {            
            try{
                sManager.postServer(newServer);
                return Ok();
            }
            catch (System.Data.SQLite.SQLiteException)
            {
                return BadRequest("ID Alread in DB");
            }
            
        }

        // DELETE: api/servers/{id}
        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            sManager.deleteServer(id);
            return Ok();
        }
    }
}