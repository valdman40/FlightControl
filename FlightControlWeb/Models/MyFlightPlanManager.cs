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
        IDataManager DataMan;
        public MyFlightPlanManager(IDataManager datamanager)
        {
            DataMan = datamanager;
        }
        public FlightPlan getFlightPlan(string id)    
        {
                // get FlightPlan by id
                string getQuery_fromFlightPlans = "SELECT * FROM FlightPlans WHERE ID ='" + id + "'";
                // var result = cnn.Query(getQuery_fromFlightPlans, new DynamicParameters());
                var result = DataMan.ExcuteQuery(getQuery_fromFlightPlans);
                if (result.Count() == 0)
                {
                    // this FlightPlan doesnt exist
                    return new FlightPlan() { };
                }

                //var flightPlan = result.ToList()[0];
                var flightPlan = result[0];
                // get Initial_Location by flightPlan.Initial_Location_ID
                string getQuery_fromInitialLocations = "SELECT Longitude,Latitude,Date FROM Initial_Locations WHERE ID ='" + flightPlan.Initial_Location_ID + "'";
                var initialLocationOb = DataMan.ExcuteQuery(getQuery_fromInitialLocations)[0];
                Initial_Location initial_Location = new Initial_Location()
                {
                    latitude = initialLocationOb.Latitude,
                    longitude = initialLocationOb.Longitude,
                    date_time = DateTime.Parse(initialLocationOb.Date)
                };

                // get segments by id and create Segments list
                string getQuery_fromSegments = "SELECT Longitude,Latitude,Timespan FROM Segments WHERE Flight_ID ='" + id + "'";
                var allSegmentsOb = DataMan.ExcuteQuery(getQuery_fromSegments);
                List<Segment> segments = new List<Segment>();
                foreach (var segmentOb in allSegmentsOb)
                {
                    segments.Add(new Segment() 
                    {
                        latitude = segmentOb.Latitude,
                        longitude = segmentOb.Longitude,
                        timespan_seconds = (int)segmentOb.Timespan
                    });
                }

                return new FlightPlan()
                {
                    company_name = flightPlan.Company,
                    passengers = (int)flightPlan.Passengers,
                    initial_location = initial_Location,
                    segments = segments.ToArray()
                };
            
        }

        public FlightPlan addFlightPlan(FlightPlan flightPlan)
        {
            // generate ID function
            string uniqueID = generateUniqueID(flightPlan.company_name);
                // post into Initial_Locations table
                Initial_Location initial_Location = flightPlan.initial_location;
                string dateString = "'"+initial_Location.date_time.ToString("yyyy-MM-ddTHH:mm:ssZ")+ "'";
                string postQuery_Initial_Locations = "INSERT INTO Initial_Locations(Longitude, Latitude, Date)"+
                                                                  " VALUES(@longitude, @latitude, " + dateString + ")";
                DataMan.ExcuteQuery(postQuery_Initial_Locations, initial_Location);

                // post into FlightPlans table
                string getQuery_extractInitialLocationID = "SELECT ID FROM Initial_Locations ORDER BY ID DESC LIMIT 1";
                var x = DataMan.ExcuteQuery(getQuery_extractInitialLocationID);
                int lastIDFromInitialLocations = (int)x.ToList()[0].ID;
                string postQuery_FlightPlans = "INSERT INTO FlightPlans(ID, Company, Passengers, Initial_Location_ID)" +
                                                               " VALUES('"+ uniqueID + "',@company_name, @passengers, " + lastIDFromInitialLocations + ")";
                DataMan.ExcuteQuery(postQuery_FlightPlans, flightPlan);
                // post into Segments table
                string postQuery_Segment = "";
                foreach(Segment segment in flightPlan.segments)
                {
                    postQuery_Segment = "INSERT INTO Segments(Longitude, Latitude, TimeSpan, Flight_ID)" +
                                                     " VALUES(@longitude, @latitude, @timespan_seconds, '" + uniqueID + "')";
                    DataMan.ExcuteQuery(postQuery_Segment, segment);
                }
            return flightPlan;
        }

        // generates unique id with addition letters from stringBase
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
