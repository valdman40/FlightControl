using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

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
                return new FlightPlan() { ID = y.ID, Company = y.Company, Passangers = (int)y.Passangers };
            }
        }

        public void addFlightPlan(FlightPlan flightPlan)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                string postQuery = "INSERT INTO Server (ID, Company, Passengers) VALUES('" + flightPlan.ID + "', " +
                    "'" + flightPlan.Company + "', '" + flightPlan.Passangers +")";
                if (cnn.Execute(postQuery) != 1)
                {
                    Console.WriteLine("failed Posting flightPlan: " + flightPlan);
                }
            }
        }

        private static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
    }
}
