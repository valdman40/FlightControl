using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using FlightControlWeb;
using FlightControlWeb.Controllers;
using FlightControlWeb.Models;
using FlightControlWeb.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace FlightControlWeb.Tests
{
    public class MyFlightManagerTest
    {
        private Mock<IFlightPlanManager> fpm { get; set; }
        private Mock<IDataManager> data { get; set; }
        [Fact]
        public void _postServerTest()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Server y = new Server() { ID = "test", URL = "test" };
                mock.Mock<IDataManager>()
                    .Setup(x => x.ExcuteQuery<Server>("INSERT INTO Servers(ID,URL)" +
                                                  " VALUES(@ID, @URL)", y)).Returns(getSampleServerList());


                var cls = mock.Create<MyServersManager>();
                var expected = getSampleServerList();
                var actual = cls.postServer(y);
                Assert.True(actual != null);
                Assert.Equal(expected.Count, actual.Count);
                for (int i = 0; i < expected.Count; i++)
                {
                    Assert.Equal(expected[i].ID, actual[i].ID);
                    Assert.Equal(expected[i].URL, actual[i].URL);
                }

            }
        }

        private List<dynamic> getSampleServerList()
        {
            List<dynamic> output = new List<dynamic>
            {
               new Server() { ID = "test", URL = "test" }
            };

            return output;
        }

        [Fact]
        public void _getServersTest()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IDataManager>()
                .Setup(x => x.ExcuteQuery("SELECT * FROM Servers")).Returns(getSampleServerList2);
                var cls = mock.Create<MyServersManager>();
                var expected = getSampleServerList2();
                var actual = cls.getServers();
                Assert.True(actual != null);
                Assert.Equal(expected.Count, actual.Count);
                for (int i = 0; i < expected.Count; i++)
                {
                    Assert.Equal(expected[i].ID, actual[i].ID);
                    Assert.Equal(expected[i].URL, actual[i].URL);
                }
            }
        }

        [Fact]

        public void _FlightPlanControllerGetTest()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var x = new Mock<IFlightPlanManager>();
                x.Setup(x => x.getFlightPlan("123")).Returns(new FlightPlan() { company_name = "sd" });
                FlightPlanController f = new FlightPlanController(x.Object);
                var actual = f.Get("123");
                var expected = new FlightPlan() { company_name = "sd" };
                Assert.True(actual != null);
                Assert.Equal(expected.company_name, actual.company_name);
            }
        }
        [Fact]
        public void _FlightPlanControllerPostTest()
        {
            using (var mock = AutoMock.GetLoose())
            {
                FlightPlan fp = new FlightPlan() { company_name = "testCompany" };
                var x = new Mock<IFlightPlanManager>();
                x.Setup(x => x.addFlightPlan(fp)).Returns(fp);
                FlightPlanController f = new FlightPlanController(x.Object);
                var actual = f.Post(fp);
                var expected = fp;
                Assert.True(actual != null);
                Assert.Equal(expected.company_name, actual.company_name);
            }
        }
        private List<Flight> getSampleFlights()
        {
            List<Flight> output = new List<Flight>
            {
               new Flight() { flight_id = "1" },
               new Flight() { flight_id = "2" },
               new Flight() { flight_id = "3" }

            };
            return output;
        }

        private List<dynamic> getSampleServerList2()
        {
            List<dynamic> output = new List<dynamic>
            {
               new Server() { ID = "1", URL = "1" },
               new Server() { ID = "2", URL = "2" },
               new Server() { ID = "3", URL = "3" }

            };

            return output;
        }
        [Fact]
        public void ShouldGetAllFlights()
        {
            // Arrange
            DateTime dateTime = DateTime.Parse("2018-09-24T04:26:19Z").ToUniversalTime();
            var x = new Mock<IFlightManager>();
            x.Setup(x => x.GetAllFlights("2018-09-24T04:26:19Z")).ReturnsAsync(getSampleFlights());

            FlightsController flightsController = new FlightsController(x.Object);

            var httpContext = new DefaultHttpContext();

            httpContext.Request.HttpContext.Request.QueryString = new QueryString("?relative_to=2018-09-24T04:26:19Z&sync_all"); ;

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            //assign context to controller
            FlightsController controller = new FlightsController(x.Object)
            {
                ControllerContext = controllerContext,
            };

            //Act
            var actual = controller.GetAllFlights().Value;
            var excpected = getSampleFlights();
            // Assert
            Assert.True(actual != null);
            Assert.Equal(excpected.Count, actual.Count);
            for (int i = 0; i < excpected.Count; i++)
            {
                Assert.Equal(excpected[i].flight_id, actual[i].flight_id);
            }
        }
        [Fact]
        public void ShouldGetInternalFlights()
        {
            // Arrange
            DateTime dateTime = DateTime.Parse("2018-09-24T04:26:19Z").ToUniversalTime();
            var x = new Mock<IFlightManager>();
            x.Setup(x => x.GetInternalFlights("2018-09-24T04:26:19Z")).Returns(getSampleFlights());

            FlightsController flightsController = new FlightsController(x.Object);

            var httpContext = new DefaultHttpContext();

            httpContext.Request.HttpContext.Request.QueryString = new QueryString("?relative_to=2018-09-24T04:26:19Z"); ;

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            //assign context to controller
            FlightsController controller = new FlightsController(x.Object)
            {
                ControllerContext = controllerContext,
            };

            //Act
            var actual = controller.GetAllFlights().Value;
            var excpected = getSampleFlights();
            // Assert
            Assert.True(actual != null);
            Assert.Equal(excpected.Count, actual.Count);
            for (int i = 0; i < excpected.Count; i++)
            {
                Assert.Equal(excpected[i].flight_id, actual[i].flight_id);
            }
        }

    }




}

