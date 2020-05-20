using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public class MyFlightManager : IFlightManager
    {
        public static List<Flight> GetInternalFlights(string date)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                string query = "SELECT * FROM Flight WHERE date_time = '" + date + "' AND is_external = 0";
                var x = cnn.Query(query, new DynamicParameters());
                var y = x.ToList();
                int len = (int)y.Count();
                List<Flight> output = new List<Flight>();
                for (int i = 0; i < len; i++)
                {
                    output.Add(new Flight()
                    {
                        Flight_id = y[i].ID,
                        Company_name = y[i].Company,
                        Date_time = date,
                        Is_external = false
                    });
                }
                return output;
            }
        }
        public static List<Flight> GetAllFlights(string date)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                string query = "SELECT * FROM Flight WHERE date_time = '" + date + "'";
                var x = cnn.Query(query, new DynamicParameters());
                var y = x.ToList();
                int len = (int)y.Count();
                List<Flight> output = new List<Flight>();
                for (int i = 0; i < len; i++)
                {
                    output.Add(new Flight()
                    {
                        Flight_id = y[i].ID,
                        Company_name = y[i].Company,
                        Date_time = date,
                        Is_external = true
                    });
                }
                return output;
            }
        }

        public static void deleteFlight(int id)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                string deleteQuery = "DELETE from Flight WHERE ID = " + id;
                if (cnn.Execute(deleteQuery) != 1)
                {
                    Console.WriteLine("failed deleting ID: " + id);
                }
            }
            
        }

        public static Flight getFlight(int id)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                string query = "SELECT * FROM Flight WHERE ID =" + id;
                var x = cnn.Query(query, new DynamicParameters());
                var y = x.ToList()[0];
                return new Flight() { Flight_id = y.ID, Company_name = y.Company, Date_time = y.date_time, Is_external = false };
            }
        }

        private static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
    }
}
