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
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FlightControlWeb.Models
{
    public class MyFlightManager : IFlightManager
    {
        public List<Flight> GetInternalFlights(string date)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                string query = "SELECT * FROM FlightPlans";
                var rowsFromFlightPlans = cnn.Query(query, new DynamicParameters()).ToList();
                List<Flight> output = new List<Flight>();
                DateTime dt = DateTime.Parse(date);
                foreach (var row in rowsFromFlightPlans)
                {
                    Initial_Location location = getLocation(dt, row.ID, (int)row.Initial_Location_ID, cnn);
                    if(location != null)
                    {
                        output.Add(new Flight()
                        {
                            flight_id = row.ID,
                            latitude = location.latitude,
                            longitude = location.longitude,
                            passengers = (int)row.Passengers,
                            company_name = row.Company,
                            date_time = dt,
                            is_external = false
                        });

                    }
                    
                }
                return output;
            }
        }

        private Initial_Location getLocation(DateTime giventime ,string flightPlan_ID, int initialLocation_ID, IDbConnection cnn)
        {
            string SegmentsQuery = "SELECT * FROM Segments WHERE Flight_ID = '" + flightPlan_ID+ "'";
            string InitialLocQuery = "SELECT * FROM Initial_Locations WHERE ID = " + initialLocation_ID;
            var rowsFromSegments = cnn.Query(SegmentsQuery, new DynamicParameters()).ToList();
            var InitialLoc= cnn.Query(InitialLocQuery, new DynamicParameters()).ToList()[0];
            List<Segment> segments = new List<Segment>();
            foreach (var row in rowsFromSegments)
            {
                segments.Add(new Segment()
                {
                    latitude = row.Latitude,
                    longitude = row.Longitude,
                    timespan_seconds = (int)row.Timespan
                }); 
            }

            DateTime departtime = DateTime.Parse(InitialLoc.Date);
            Segment startSegment = new Segment()
            {
                longitude = InitialLoc.Longitude,
                latitude = InitialLoc.Latitude,
                timespan_seconds = 0
            };
            DateTime landtime = DateTime.Parse(InitialLoc.Date);
            foreach(Segment endSegment in segments)
            {
                   landtime = landtime.AddSeconds(endSegment.timespan_seconds);
                // check if giventime is in range
                if (giventime.CompareTo(departtime) >= 0 && giventime.CompareTo(landtime) <= 0)
                {
                    TimeSpan secondsOnAir = giventime.Subtract(departtime);
                    double progress = secondsOnAir.TotalSeconds / endSegment.timespan_seconds; // between 0-1
                    Line line = new Line();
                    // x is longitude, y is latitude
                    line.start = new Point() { X = startSegment.longitude, Y = startSegment.latitude };
                    line.end = new Point() { X = endSegment.longitude, Y = endSegment.latitude };
                    Point onSegment = line.getPointOnSegment(progress);
                    return new Initial_Location()
                    {
                        latitude = onSegment.Y,
                        longitude = onSegment.X,
                        date_time = giventime
                    };
                }
                departtime = landtime; // try with next round
                startSegment = endSegment;
            }

            return null;
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
