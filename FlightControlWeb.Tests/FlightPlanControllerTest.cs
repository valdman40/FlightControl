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
    public class FlightPlanControllerTest
    {
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
    }
}
