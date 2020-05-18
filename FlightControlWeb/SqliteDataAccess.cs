using Dapper;
using FlightControlWeb.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace FlightControlWeb
{
    public class SqliteDataAccess
    {
        public static List<Flight> LoadFlights()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<Flight>("select * from Flight", new DynamicParameters());
                return output.ToList();
            }
        }
        public static void SaveFlight(Flight sflight)

        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("insert into Flight (ID, Company) values (@Flight_id, @Company_name)", sflight);
            }

        }
        private static string LoadConnectionString(string id= "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
    }
}
