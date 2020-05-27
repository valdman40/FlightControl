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
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using Newtonsoft.Json;

namespace FlightControlWeb.Models
{
    
    public class MyFlightManager : IFlightManager
    {
        IDataManager DataMan;
        public MyFlightManager(IDataManager datamanager)
        {
            DataMan = datamanager;
        }
        private static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }

        private Initial_Location getLocation(DateTime giventime, string flightPlan_ID, int initialLocation_ID)
        {
            string SegmentsQuery = "SELECT * FROM Segments WHERE Flight_ID = '" + flightPlan_ID + "'";
            string InitialLocQuery = "SELECT * FROM Initial_Locations WHERE ID = " + initialLocation_ID;
            var rowsFromSegments = DataMan.ExcuteQuery(SegmentsQuery);
            var InitialLoc = DataMan.ExcuteQuery(InitialLocQuery)[0];
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
          
                string query = "SELECT * FROM FlightPlans";
                var rowsFromFlightPlans = DataMan.ExcuteQuery(query);
                List<Flight> internalFlights = new List<Flight>();
                DateTime dt = DateTime.Parse(date);
                foreach (var row in rowsFromFlightPlans)
                {
                    Initial_Location location = getLocation(dt, row.ID, (int)row.Initial_Location_ID);
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

        public async Task<List<Flight>> GetAllFlights(string date)
        {
            List<Flight> flightList = GetInternalFlights(date);

            // get Flights from other servers and add to flightList
            var servers = (dynamic)null;
                string query = "SELECT URL FROM Servers";
                var result = DataMan.ExcuteQuery(query);
                if (result.Count() == 0)
                {
                    // doesnt have servers
                    return null;
                }
                servers = result.ToList();
            
            foreach(var server in servers) // fetch from each server
            {
                string uri = server.URL + "/api/Flights?relative_to=" + date;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                HttpClient client = new HttpClient();

                HttpResponseMessage response = await client.GetAsync(uri);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                try
                {
                    if (response.IsSuccessStatusCode && !responseBody.Contains("fail"))
                    {
                        // get all flights into list and then copy the elements into flightList
                        List<Flight> externalFlights = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Flight>>(responseBody);
                        DateTime dt = DateTime.Parse(date);
                        foreach (var exFlight in externalFlights)
                        {
                            flightList.Add(new Flight()
                            {
                                flight_id = exFlight.flight_id,
                                latitude = exFlight.latitude,
                                longitude = exFlight.longitude,
                                passengers = exFlight.passengers,
                                company_name = exFlight.company_name,
                                date_time = dt,
                                is_external = true
                            });
                        }
                    }
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine(e);
                }

                client.Dispose();
                
            }
            return flightList;
        }

        // delete flight by id from 3 different tables: FlightPlans, Segments, Initial_Locations
        public void deleteFlight(string id)
        {

                // get initial location ID by flightplan's id from FlightPlans table
                string getQuery_extractInitialLocationID = "SELECT Initial_Location_ID FROM FlightPlans WHERE ID = '" + id + "'";
                var result = DataMan.ExcuteQuery(getQuery_extractInitialLocationID);
                if (result.Count() == 0)
                {
                    // this Flight doesnt exist
                    return;
                }
                var initialLocationRow = result[0];
                int IDFromInitialLocations = (int)initialLocationRow.Initial_Location_ID;

                // delete the row from FlightPlans table
                string deleteFromFlightPlansQuery = "DELETE from FlightPlans WHERE ID = '" + id+ "'";
                DataMan.ExcuteQuery(deleteFromFlightPlansQuery);
                // delete the row from Segments table
                string deleteFromSegmentsQuery = "DELETE from Segments WHERE Flight_ID = '" + id+ "'";
                DataMan.ExcuteQuery(deleteFromSegmentsQuery);
                // delete the row from Initial_Locations table
                string DeleteFromInitialLocQuery = "DELETE from Initial_Locations WHERE ID = " + IDFromInitialLocations;
                DataMan.ExcuteQuery(DeleteFromInitialLocQuery);
            
            
        }

    }

}
