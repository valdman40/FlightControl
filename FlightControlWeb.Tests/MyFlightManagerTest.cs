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
        [Fact]
        public void _getServersTest()
        {
            using (var mock = AutoMock.GetLoose())
            {
              //  Server y = new Server() { ID = "test", URL = "test" };
                mock.Mock<IDataManager>()
                    .Setup(x => x.ExcuteQuery("SELECT * FROM Server")).Returns(getSampleServerList2());
                mock.Mock<IDataManager>()
    .Setup(x => x.ExcuteQuery("SELECT * FROM Servers")).Returns(getSampleServerList);


                var cls = mock.Create<MyServersManager>();
                var expected = getSampleServerList2();
                var actual = cls.getServers();
                Assert.True(actual != null);
                Assert.Equal(expected.Count, actual.Count);
                for(int i = 0; i < expected.Count; i++)
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
    }
}
