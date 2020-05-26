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
        public void Post([FromBody] Server newServer)
        {
            sManager.postServer(newServer);
        }

        // DELETE: api/servers/{id}
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            sManager.deleteServer(id);
        }
    }
}