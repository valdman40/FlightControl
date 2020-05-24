using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Types
{
    public class Server
    {
        private string serverId;
        public string ID { get { return serverId; } set { serverId = value; } }
        private string serverUrl;
        public string URL{ get { return serverUrl; } set { serverUrl = value; } }
    }
}
