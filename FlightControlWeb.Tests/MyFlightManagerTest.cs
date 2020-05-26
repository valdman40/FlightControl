using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Autofac.Extras.Moq;
using FlightControlWeb;
using FlightControlWeb.Controllers;
using FlightControlWeb.Models;
using FlightControlWeb.Types;
using Xunit;

namespace FlightControlWeb.Tests
{
    public class MyFlightManagerTest
    {
        [Fact]
        public void _postServerFuncTest()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Server y = new Server() { ID = "test", URL = "test" };
                mock.Mock<IDataManager>()
                    .Setup(x => x.ExcuteQuery<Server>("INSERT INTO Servers(ID,URL)" +
                                                  " VALUES(@ID, @URL)", y)).Returns(getSampleFlightList());
   
        
                var cls = mock.Create<MyServersManager>();
                var expected = getSampleFlightList();
                var actual = cls.postServer(y);
                Assert.True(actual != null);
              
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
    }
}
