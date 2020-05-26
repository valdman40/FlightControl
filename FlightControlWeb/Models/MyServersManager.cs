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
        public List<dynamic> getServers()    //this method was static by mistake?                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    -+

        {

                string query = "SELECT * FROM Server";
                var servers = DataMan.ExcuteQuery(query);
            List<dynamic> output = new List<dynamic>();
                foreach(var serverVar in servers)
                {
                    output.Add(new Server() { ID = serverVar.ID, URL = serverVar.URL });
                }
                return output;
            
        }

        public List<dynamic> postServer(Server server)
        {

                string postQuery_Server = "INSERT INTO Servers(ID,URL)" +
                                                  " VALUES(@ID, @URL)";
           
            var x = DataMan.ExcuteQuery(postQuery_Server, server);
            return x;

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
