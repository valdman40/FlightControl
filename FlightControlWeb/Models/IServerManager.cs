﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public interface IServerManager
    {
        List<Server> getServers();
        void postServer(Server server);
    }
}
