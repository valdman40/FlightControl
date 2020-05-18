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
using Microsoft.VisualStudio.Web.CodeGeneration.EntityFrameworkCore;

namespace FlightControlWeb
{
    public class SqliteDataAccess
    {
        public static List<Flight> LoadFlights()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var x = cnn.Query("select * from Flight", new DynamicParameters());
                var y = x.ToList();
                int len = (int) y.Count();
                List<Flight> output = new List<Flight>();
                for (int i = 0; i < len; i++)
                {
                    output.Add(new Flight() { Flight_id = (int) y[i].ID, Company_name = y[i].Company });
                }
                return output;
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
