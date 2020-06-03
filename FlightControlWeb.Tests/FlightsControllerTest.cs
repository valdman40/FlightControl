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
    public class FlightsControllerTest
    {
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
