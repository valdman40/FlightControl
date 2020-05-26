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
        public List<Server> getServers()    //this method was static by mistake?                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    -+

        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                string query = "SELECT * FROM Server";
                var servers = cnn.Query(query, new DynamicParameters()).ToList();
                List<Server> output = new List<Server>();
                foreach(var serverVar in servers)
                {
                    output.Add(new Server() { ID = serverVar.ID, URL = serverVar.URL });
                }
                return output;
            }
        }

        public void postServer(Server server)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                string postQuery_Server = "INSERT INTO Servers(ID,URL)" +
                                                  " VALUES(@ID, @URL)";
                cnn.Execute(postQuery_Server, server);
            }

        }

        private static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }

        public void deleteServer(string id)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                string deleteQuery = "DELETE from Servers WHERE ID = '" + id+"'";
                cnn.Execute(deleteQuery, new DynamicParameters());
            }
        }
    }
}
