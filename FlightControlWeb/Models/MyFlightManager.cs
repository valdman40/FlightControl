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
        private static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }

        private Initial_Location getLocation(DateTime giventime, string flightPlan_ID, int initialLocation_ID, IDbConnection cnn)
        {
            string SegmentsQuery = "SELECT * FROM Segments WHERE Flight_ID = '" + flightPlan_ID + "'";
            string InitialLocQuery = "SELECT * FROM Initial_Locations WHERE ID = " + initialLocation_ID;
            var rowsFromSegments = cnn.Query(SegmentsQuery, new DynamicParameters()).ToList();
            var InitialLoc = cnn.Query(InitialLocQuery, new DynamicParameters()).ToList()[0];
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
            foreach (Segment endSegment in segments)
            {
                landtime = landtime.AddSeconds(endSegment.timespan_seconds); // adding the next segment duration
                // check if giventime is in range
                if (giventime.CompareTo(departtime) >= 0 && giventime.CompareTo(landtime) <= 0)
                {
                    TimeSpan secondsOnAir = giventime.Subtract(departtime); // total seconds from last departing untill now
                    double progress = secondsOnAir.TotalSeconds / endSegment.timespan_seconds; // between 0-1
                    Line line = new Line();
                    // x is longitude, y is latitude
                    line.start = new Point() { X = startSegment.longitude, Y = startSegment.latitude };
                    line.end = new Point() { X = endSegment.longitude, Y = endSegment.latitude };
                    Point onSegment = line.getPointOnSegment(progress);
                    return new Initial_Location() // location on the current segment
                    {
                        latitude = onSegment.Y,
                        longitude = onSegment.X,
                        date_time = giventime
                    };
                }
                departtime = landtime;
                startSegment = endSegment;
            }

            return null; // wasn't in any segment so return null
        }

        public List<Flight> GetInternalFlights(string date)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                string query = "SELECT * FROM FlightPlans";
                var rowsFromFlightPlans = cnn.Query(query, new DynamicParameters()).ToList();
                List<Flight> internalFlights = new List<Flight>();
                DateTime dt = DateTime.Parse(date);
                foreach (var row in rowsFromFlightPlans)
                {
                    Initial_Location location = getLocation(dt, row.ID, (int)row.Initial_Location_ID, cnn);
                    if(location != null) // means that this FlightPlan is active
                    {
                        internalFlights.Add(new Flight()
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
                return internalFlights;
            }
        }

        public  List<Flight> GetAllFlights(string date)
        {
            List<Flight> flightList = GetInternalFlights(date);
            // need to get from other servers and add it to flightList

       
            return flightList;
        }

        // delete flight by id from 3 different tables: FlightPlans, Segments, Initial_Locations
        public void deleteFlight(string id)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                // get initial location ID by flightplan's id from FlightPlans table
                string getQuery_extractInitialLocationID = "SELECT Initial_Location_ID FROM FlightPlans WHERE ID = '" + id + "'";
                var initialLocationRow = cnn.Query(getQuery_extractInitialLocationID, new DynamicParameters()).ToList()[0];
                int IDFromInitialLocations = (int)initialLocationRow.Initial_Location_ID;

                // delete the row from FlightPlans table
                string deleteFromFlightPlansQuery = "DELETE from FlightPlans WHERE ID = '" + id+ "'";
               cnn.Query(deleteFromFlightPlansQuery, new DynamicParameters());

                // delete the row from Segments table
                string deleteFromSegmentsQuery = "DELETE from Segments WHERE Flight_ID = '" + id+ "'";
                cnn.Query(deleteFromSegmentsQuery, new DynamicParameters());

                // delete the row from Initial_Locations table
                string DeleteFromInitialLocQuery = "DELETE from Initial_Locations WHERE ID = " + IDFromInitialLocations;
                cnn.Query(DeleteFromInitialLocQuery, new DynamicParameters());
            }
            
        }

        /*
         * this function finds the FlightPlan (by ID) in the DB and generates a Flight Object accordingly
         */
        public Flight getFlight(string id)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                string query = "SELECT * FROM FlightPlan WHERE ID =" + id;
                var x = cnn.Query(query, new DynamicParameters());
                var y = x.ToList()[0];
                return new Flight() { flight_id = y.ID, company_name = y.Company, date_time = DateTime.Parse(y.date_time), is_external = false };
            }
        }


    }

}
