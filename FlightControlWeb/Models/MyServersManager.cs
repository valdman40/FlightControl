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
                var x = cnn.Query(query, new DynamicParameters());
                var y = x.ToList();
                int len = (int)y.Count();
                List<Server> output = new List<Server>();
                for (int i = 0; i < len; i++)
                {
                    output.Add(new Server(){ ID = y[i].ID, URL = y[i].URL });
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
                /*
                string postQuery = "INSERT INTO Server (ID, URL) VALUES('"+server.ID+ "', '" + server.URL + "')";
                if (cnn.Execute(postQuery) != 1)
                {
                    Console.WriteLine("failed Posting server: " + server);
                }
                */
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
                /*
                if (cnn.Execute(deleteQuery) != 1)
                {
                    Console.WriteLine("failed deleting ID: " + id);
                }
                */
            }
        }
    }
}
