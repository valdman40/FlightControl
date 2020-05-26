using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using FlightControlWeb.Types;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public class MyServersManager : IServerManager 
    {
        IDataManager DataMan;
        public MyServersManager(IDataManager datamanager)
        {
            DataMan = datamanager;
        }
        public List<Server> getServers()    //this method was static by mistake?                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    -+

        {

                string query = "SELECT * FROM Server";
                var servers = DataMan.ExcuteQuery(query);
                List<Server> output = new List<Server>();
                foreach(var serverVar in servers)
                {
                    output.Add(new Server() { ID = serverVar.ID, URL = serverVar.URL });
                }
                return output;
            
        }

        public void postServer(Server server)
        {

                string postQuery_Server = "INSERT INTO Servers(ID,URL)" +
                                                  " VALUES(@ID, @URL)";
            DataMan.ExcuteQuery(postQuery_Server, server);

        }

        private static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }

        public void deleteServer(string id)
        {

            string deleteQuery = "DELETE from Servers WHERE ID = '" + id+"'";
            DataMan.ExcuteQuery(deleteQuery);
            
        }
    }
}
