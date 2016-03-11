using DistanceTracker.API.Controllers;
using DistanceTracker.API.ViewModel;
using DistanceTracker.Data.Model;
using DistanceTracker.Data.UnitOfWork;
using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DistanceTracker.Web.Tests.API
{
    public class RouteControllerTests : ControllerTestsBase<RouteController, SQLiteDbFixture>, IClassFixture<SQLiteDbFixture>
    {
        public RouteControllerTests(SQLiteDbFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public void Get_returns_route_if_exists()
        {
            var user = GetTestUser("user1");
            using (var ctrl = GetController(user))
            {
                // Arrange 
                AddSimpleRoute(ctrl, user.Id);

                // Act
                var result = ctrl.Get();

                // Assert
                var resultValue = ((HttpOkObjectResult)result).Value as DistanceTracker.API.ViewModel.Route;
                Assert.True(result is HttpOkObjectResult);
                Assert.True(resultValue != null);
                Assert.True(resultValue.StartLocation.Lat == 1.1);
                Assert.True(resultValue.StartLocation.Lng == 1.2);
                Assert.True(resultValue.EndLocation.Lat == 2.1);
                Assert.True(resultValue.EndLocation.Lng == 2.2);
                Assert.True(resultValue.Waypoints.Count() == 2);
                Assert.True(resultValue.Waypoints.ToArray()[0].Lat == 11.1);
                Assert.True(resultValue.Waypoints.ToArray()[0].Lng == 11.2);
                Assert.True(resultValue.Waypoints.ToArray()[1].Lat == 22.1);
                Assert.True(resultValue.Waypoints.ToArray()[1].Lng == 22.2);
            }
        }

        [Fact]
        public void Get_returns_default_route_if_doesnt_exist()
        {
            using (var ctrl = GetController(GetTestUser("user2")))
            {
                // Act
                var result = ctrl.Get();

                // Assert
                var resultValue = ((HttpOkObjectResult)result).Value as DistanceTracker.API.ViewModel.Route;
                Assert.True(result is HttpOkObjectResult);
                Assert.True(resultValue != null);
                Assert.True(resultValue.StartLocation.Lat == 60.1728931);
                Assert.True(resultValue.StartLocation.Lng == 24.941188199999942);
                Assert.True(resultValue.EndLocation.Lat == 60.4519805);
                Assert.True(resultValue.EndLocation.Lng == 22.26639679999994);
                Assert.True(resultValue.Waypoints == null);
            }
        }

        [Fact]
        public void Post_updates_route_if_exists()
        {
            var user = GetTestUser("user3");
            using (var ctrl = GetController(user))
            {
                // Arrange
                var oriRoute = AddSimpleRoute(ctrl, user.Id);

                // Act
                var result = ctrl.Post(new DistanceTracker.API.ViewModel.Route
                {
                    StartLocation = new Point { Lat = 3.1, Lng = 3.2 },
                    EndLocation = new Point { Lat = 4.1, Lng = 4.2 },
                    Waypoints = new []
                    {
                        new Point { Lat = 33.1, Lng = 33.2 }
                    }
                });

                // Assert
                var route = ctrl.UnitOfWork.Routes.Find(x => x.UserId == user.Id).FirstOrDefault();
                Assert.True(result is HttpOkResult);
                Assert.True(route != null);
                Assert.True(oriRoute.Id == route.Id);
                Assert.True(route.StartLocation.ToPoint().Lat == 3.1);
                Assert.True(route.StartLocation.ToPoint().Lng == 3.2);
                Assert.True(route.EndLocation.ToPoint().Lat == 4.1);
                Assert.True(route.EndLocation.ToPoint().Lng == 4.2);
                Assert.True(route.Waypoints.ToPoints().Count() == 1);
                Assert.True(route.Waypoints.ToPoints().ToArray()[0].Lat == 33.1);
                Assert.True(route.Waypoints.ToPoints().ToArray()[0].Lng == 33.2);
            }
        }

        [Fact]
        public void Post_creates_route_if_doesnt_exist()
        {
            var user = GetTestUser("user4");
            using (var ctrl = GetController(user))
            {
                // Act
                var result = ctrl.Post(new DistanceTracker.API.ViewModel.Route
                {
                    StartLocation = new Point { Lat = 3.1, Lng = 3.2 },
                    EndLocation = new Point { Lat = 4.1, Lng = 4.2 },
                    Waypoints = new[]
                    {
                        new Point { Lat = 33.1, Lng = 33.2 }
                    }
                });

                // Assert
                var route = ctrl.UnitOfWork.Routes.Find(x => x.UserId == user.Id).FirstOrDefault();
                Assert.True(result is HttpOkResult);
                Assert.True(route != null);
                Assert.True(route.StartLocation.ToPoint().Lat == 3.1);
                Assert.True(route.StartLocation.ToPoint().Lng == 3.2);
                Assert.True(route.EndLocation.ToPoint().Lat == 4.1);
                Assert.True(route.EndLocation.ToPoint().Lng == 4.2);
                Assert.True(route.Waypoints.ToPoints().Count() == 1);
                Assert.True(route.Waypoints.ToPoints().ToArray()[0].Lat == 33.1);
                Assert.True(route.Waypoints.ToPoints().ToArray()[0].Lng == 33.2);
            }
        }

        [Fact]
        public void Delete_removes_route_if_exists()
        {
            var user = GetTestUser("user5");
            using (var ctrl = GetController(user))
            {
                // Arrange
                AddSimpleRoute(ctrl, user.Id);

                // Act
                var result = ctrl.Delete();

                // Assert
                Assert.True(result is HttpOkResult);
                Assert.True(ctrl.UnitOfWork.Routes.Find(x => x.UserId == user.Id).Count() == 0);
            }
        }

        [Fact]
        public void Delete_does_nothing_if_route_doesnt_exist()
        {
            var user = GetTestUser("user6");
            using (var ctrl = GetController(user))
            {
                // Act
                var result = ctrl.Delete();

                // Assert
                Assert.True(result is HttpOkResult);
                Assert.True(ctrl.UnitOfWork.Routes.Find(x => x.UserId == user.Id).Count() == 0);
            }
        }

        private Data.Model.Route AddSimpleRoute(RouteController ctrl, int userId)
        {
            var route = new Data.Model.Route
            {
                UserId = userId,
                StartLocation = (new Point { Lat = 1.1, Lng = 1.2 }).PointToString(),
                EndLocation = (new Point { Lat = 2.1, Lng = 2.2 }).PointToString(),
                Waypoints = (new[]
                {
                    new Point { Lat = 11.1, Lng = 11.2 },
                    new Point { Lat = 22.1, Lng = 22.2 }
                }).PointsToString()
            };

            ctrl.UnitOfWork.Routes.Add(route);
            ctrl.UnitOfWork.Complete();

            return route;
        }
    }
}
