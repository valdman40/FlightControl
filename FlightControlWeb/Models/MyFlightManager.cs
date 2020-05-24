using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using FlightControlWeb.Types;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public class MyFlightManager : IFlightManager
    {
        public List<Flight> GetInternalFlights(string date)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                string query = "SELECT * FROM FlightPlan WHERE date_time = '" + date + "' AND is_external = 0";
                var x = cnn.Query(query, new DynamicParameters());
                var y = x.ToList();
                int len = (int)y.Count();
                List<Flight> output = new List<Flight>();
                for (int i = 0; i < len; i++)
                {
                    output.Add(new Flight()
                    {
                        flight_id = y[i].ID,
                        company_name = y[i].Company,
                        date_time = DateTime.Parse(date),
                        is_external = false
                    });
                }
                return output;
            }
        }
        public  List<Flight> GetAllFlights(string date)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                string query = "SELECT * FROM FlightPlan WHERE date_time = '" + date + "'";
                var x = cnn.Query(query, new DynamicParameters());
                var y = x.ToList();
                int len = (int)y.Count();
                List<Flight> output = new List<Flight>();
                for (int i = 0; i < len; i++)
                {
                    output.Add(new Flight()
                    {
                        flight_id = y[i].ID,
                        company_name = y[i].Company,
                        date_time = DateTime.Parse(date),
                        is_external = true
                    });
                }
                return output;
            }
        }

        public  void deleteFlight(int id)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                string deleteQuery = "DELETE from FlightPlan WHERE ID = " + id;
                if (cnn.Execute(deleteQuery) != 1)
                {
                    Console.WriteLine("failed deleting ID: " + id);
                }
            }
            
        }

        
        /*
         * this function finds the FlightPlan (by ID) in the DB and generates a Flight Object accordingly
         */
        public Flight getFlight(int id)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                string query = "SELECT * FROM FlightPlan WHERE ID =" + id;
                var x = cnn.Query(query, new DynamicParameters());
                var y = x.ToList()[0];
                return new Flight() { flight_id = y.ID, company_name = y.Company, date_time = DateTime.Parse(y.date_time), is_external = false };
            }
        }

        private static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
    }

}
