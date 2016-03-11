using DistanceTracker.API.ViewModel;
using System.Linq;
using Xunit;

namespace DistanceTracker.Web.Tests.Extensions
{
    public class ViewModelExtensionsTests
    {
        [Fact]
        public void PointToString()
        {
            var point = new Point()
            {
                Lat = 10.111,
                Lng = 20.222
            };

            Assert.True(point.PointToString() == "{\"Lat\":10.111,\"Lng\":20.222}");
        }

        [Fact]
        public void StringToPoint()
        {
            var str = "{\"Lat\":10.111,\"Lng\":20.222}";

            var point = str.ToPoint();

            Assert.True(point.Lat == 10.111);
            Assert.True(point.Lng == 20.222);
        }

        [Fact]
        public void PointsToString()
        {
            var points = new[]
            {
                new Point()
                {
                    Lat = 10.111,
                    Lng = 20.222
                },
                new Point()
                {
                    Lat = 30.333,
                    Lng = 40.444
                },
            };

            Assert.True(points.PointsToString() == "[{\"Lat\":10.111,\"Lng\":20.222},{\"Lat\":30.333,\"Lng\":40.444}]");
        }

        [Fact]
        public void StringToPoints()
        {
            var str = "[{\"Lat\":10.111,\"Lng\":20.222},{\"Lat\":30.333,\"Lng\":40.444}]";

            var points = str.ToPoints().ToArray();

            Assert.True(points[0].Lat == 10.111);
            Assert.True(points[0].Lng == 20.222);
            Assert.True(points[1].Lat == 30.333);
            Assert.True(points[1].Lng == 40.444);
        }
    }
}
