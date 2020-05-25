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
using System.Text.RegularExpressions;

namespace FlightControlWeb.Models
{
    public class MyFlightPlanManager: IFlightPlanManager
    {
        public FlightPlan getFlightPlan(string id)    //this method was static by mistake?                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    -+
        
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                string getQuery_fromFlightPlans = "SELECT * FROM FlightPlans WHERE ID ='" + id+"'";
                string getQuery_fromSegments = "SELECT Longitude,Latitude,Timespan FROM Segments WHERE Flight_ID ='" + id + "'";
                var x = cnn.Query(getQuery_fromFlightPlans, new DynamicParameters());
                var y = x.ToList();
                string getQuery_fromInitialLocations = "SELECT Longitude,Latitude,Date FROM Initial_Locations WHERE ID ='" + y[0].Initial_Location_ID + "'";
                x = cnn.Query(getQuery_fromInitialLocations, new DynamicParameters());
                var z = x.ToList()[0];
                Initial_Location initial_Location = new Initial_Location()
                {
                    latitude = z.Latitude,
                    longitude = z.Longitude,
                    date_time = DateTime.Parse(z.Date)
                };
                x = cnn.Query(getQuery_fromSegments, new DynamicParameters());
                var a = x.ToList();
                List<Segment> segments = new List<Segment>();
                foreach (var ob in a)
                {
                    segments.Add(new Segment() 
                    {
                        latitude = ob.Latitude,
                        longitude = ob.Longitude,
                        timespan_seconds = (int)ob.Timespan
                    });
                }
                return new FlightPlan()
                {
                    company_name = y[0].Company,
                    passengers = y[0].Passengers,
                    initial_location = initial_Location,
                    segments = segments.ToArray()
                };
            }
        }

        public void addFlightPlan(FlightPlan flightPlan)
        {
            // generate ID function
            string uniqueID = generateUniqueID(flightPlan.company_name);
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                // post into Initial_Locations table
                Initial_Location initial_Location = flightPlan.initial_location;
                string dateString = "'"+initial_Location.date_time.ToString("yyyy-MM-ddTHH:mm:ssZ")+ "'";
                string postQuery_Initial_Locations = "INSERT INTO Initial_Locations(Longitude, Latitude, Date)"+
                                                                  " VALUES(@longitude, @latitude, " + dateString + ")";
                cnn.Execute(postQuery_Initial_Locations, initial_Location);

                // post into FlightPlans table
                string getQuery_extractInitialLocationID = "SELECT ID FROM Initial_Locations ORDER BY ID DESC LIMIT 1";
                var x = cnn.Query(getQuery_extractInitialLocationID, new DynamicParameters());
                int lastIDFromInitialLocations = (int)x.ToList()[0].ID;
                string postQuery_FlightPlans = "INSERT INTO FlightPlans(ID, Company, Passengers, Initial_Location_ID)" +
                                                               " VALUES('"+ uniqueID + "',@company_name, @passengers, " + lastIDFromInitialLocations + ")";
                cnn.Execute(postQuery_FlightPlans, flightPlan);
                
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

        private string generateUniqueID(string stringBase)
        {
            var abc = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            var numbers = "0123456789";
            var stringVar = new char[10];
            stringVar[0] = stringBase[0];
            Random rnd = new Random();
            if (stringBase.Length > 3)
            {
                stringVar[1] = stringBase[1];
                stringVar[2] = stringBase[2];
            } else
            {
                stringVar[1] = 'X';
                stringVar[2] = 'X';
            }
            stringVar[3] = '_';
            for (int i = 4; i < 8; i++)
            {
                stringVar[i] = abc[rnd.Next(abc.Length)];
            }
            for (int i = 8; i < 10; i++)
            {
                stringVar[i] = numbers[rnd.Next(numbers.Length)];
            }
            return new string(stringVar);
        }

        private static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
    }
}
