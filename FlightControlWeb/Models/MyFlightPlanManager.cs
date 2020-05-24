using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using FlightControlWeb.Types;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace FlightControlWeb.Models
{
    public class MyFlightPlanManager: IFlightPlanManager
    {
        public FlightPlan getFlightPlan(int id)    //this method was static by mistake?                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    -+
        
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                string query = "SELECT * FROM FlightPlan WHERE ID =" + id;
                var x = cnn.Query(query, new DynamicParameters());
                var y = x.ToList()[0];
                return new FlightPlan() { };
            }
        }

        public void addFlightPlan(FlightPlan flightPlan)
        {
            // generate ID function
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                Initial_Location initial_Location = flightPlan.initial_location;
                string postQuery_Location = "INSERT INTO FlightPlan(flight_ID, company_name, passengers, location_id) " +
    "                                          VALUES(@flight_ID,@company_name, @passengers, @location_id)";
                cnn.Execute(postQuery_Location, flightPlan);
                string postQuery_FlightPlan = "INSERT INTO FlightPlan(flight_ID, company_name, passengers, location_id) " +
                    "                                          VALUES(@flight_ID,@company_name, @passengers, @location_id)";
                cnn.Execute(postQuery_FlightPlan, flightPlan);
            }
        }

        private static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
    }
}
