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
                string query = "SELECT * FROM FlightPlans WHERE ID =" + id;
                var x = cnn.Query(query, new DynamicParameters());
                var y = x.ToList()[0];
                return new FlightPlan() { };
            }
        }

        public void addFlightPlan(FlightPlan flightPlan)
        {
            // generate ID function
            string uniqueID = "43f34sd";
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                // post into Initial_Locations table
                Initial_Location initial_Location = flightPlan.initial_location;
                string dateString = "'"+initial_Location.date_time.ToString("yyyy-MM-ddTHH:mm:ssZ")+ "'";
                string postQuery_Initial_Locations = "INSERT INTO Initial_Locations(Longitude, Latitude, Date)"+
                                                                  " VALUES(@longitude, @latitude, " + dateString + ")";
                cnn.Execute(postQuery_Initial_Locations, initial_Location); // returns 1 if success

                // post into FlightPlans table
                string getQuery_extractInitialLocationID = "SELECT ID FROM Initial_Locations ORDER BY ID DESC LIMIT 1";
                var x = cnn.Query(getQuery_extractInitialLocationID, new DynamicParameters());
                int lastIDFromInitialLocations = (int)x.ToList()[0].ID;
                string postQuery_FlightPlans = "INSERT INTO FlightPlans(ID, Company, Passengers, Initial_Location_ID)" +
                                                             " VALUES('"+ uniqueID + "',@company_name, @passengers, " + lastIDFromInitialLocations + ")";
                int i = cnn.Execute(postQuery_FlightPlans, flightPlan); // returns 1 if success
                
                // post into Segments table
                string postQuery_Segment = "";
                foreach(Segment segment in flightPlan.segments)
                {
                    postQuery_Segment = "INSERT INTO Segments(Longitude, Latitude, TimeSpan, Flight_ID)" +
                                                             " VALUES(@longitude, @latitude, @timespan_seconds, '" + uniqueID + "')";
                    cnn.Execute(postQuery_Segment, segment);
                }
            }
        }

        private static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
    }
}
