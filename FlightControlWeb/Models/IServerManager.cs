using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightControlWeb.Types;

namespace FlightControlWeb.Models
{
    public interface IServerManager
    {
        List<Server> getServers();
        List<dynamic> postServer(Server server);

        void deleteServer(string id);
    }
}
