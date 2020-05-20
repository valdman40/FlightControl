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
    public class MyFlightPlanManager
    {
        public static FlightPlan getFlightPlan(int id)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                string query = "SELECT * FROM FlightPlan WHERE ID =" + id;
                var x = cnn.Query(query, new DynamicParameters());
                var y = x.ToList()[0];
                return new FlightPlan() { ID = y.ID, Company_name = y.Company, Date = y.date_time};
            }
        }

        private static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
    }
}
