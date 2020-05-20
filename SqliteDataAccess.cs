using Dapper;
using FlightControlWeb.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Web.CodeGeneration.EntityFrameworkCore;
using System.Diagnostics;

namespace FlightControlWeb
{
    public class SqliteDataAccess
    {
        public static List<Flight> LoadFlights(string timeString, bool sync_all)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                DateTime date;
                if (!DateTime.TryParse(timeString, out date))
                {
                    Debug.WriteLine("Error: not a valid date");
                }
                string query = "SELECT * FROM Flight";
                if (sync_all)
                {
                    // need to pull other flights from other servers
                    query = "SELECT * FROM Flight WHERE date_time = '" + timeString + "'";
                } else
                {
                    query = "SELECT * FROM Flight WHERE date_time = '"+timeString+"' AND is_external = 0";
                }
                var x = cnn.Query(query, new DynamicParameters());
                var y = x.ToList();
                int len = (int) y.Count();
                List<Flight> output = new List<Flight>();
                for (int i = 0; i < len; i++)
                {
                    output.Add(new Flight() { 
                        Flight_id = y[i].ID, 
                        Company_name = y[i].Company, 
                    });
                    /*
                                         Date_time = y[i].date_time,
                        Is_external = Int32.Parse(y[i].date_time) == 1 ? true: false    
                 */
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
