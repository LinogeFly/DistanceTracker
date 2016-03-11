using System;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Authorization;
using DistanceTracker.Data.UnitOfWork;
using System.Linq;
using DistanceTracker.API.ViewModel;

namespace DistanceTracker.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class RouteController : Controller, IApiController
    {
        [FromServices]
        public IHostingEnvironment HostingEnvironment { get; set; }

        [FromServices]
        public IUnitOfWork UnitOfWork { get; set; }

        public RouteController()
        {
        }

        [HttpGet]
        public IActionResult Get()
        {
            if (HostingEnvironment.IsDevelopment())
                System.Threading.Thread.Sleep(1000);

            var user = UnitOfWork.Users.CurrentUser(User);
            if (user == null)
                return HttpUnauthorized();

            var route = UnitOfWork.Routes.Find(x => x.UserId == user.Id).FirstOrDefault();
            if (route != null)
                return Ok(new Route()
                {
                    StartLocation = route.StartLocation.ToPoint(),
                    EndLocation = route.EndLocation.ToPoint(),
                    Waypoints = route.Waypoints.ToPoints()
                });

            // Default route
            return Ok(new Route()
            {
                StartLocation = new Point { Lat = 60.1728931, Lng = 24.941188199999942 },
                EndLocation = new Point { Lat = 60.4519805, Lng = 22.26639679999994 }
            });
        }

        [HttpPost]
        public IActionResult Post([FromBody]Route newRoute)
        {
            if (HostingEnvironment.IsDevelopment())
                System.Threading.Thread.Sleep(1000);

            var user = UnitOfWork.Users.CurrentUser(User);
            if (user == null)
                return HttpUnauthorized();

            var route = UnitOfWork.Routes.Find(x => x.UserId == user.Id).FirstOrDefault();
            if (route != null) // Update existing
            {
                route.StartLocation = newRoute.StartLocation.PointToString();
                route.EndLocation = newRoute.EndLocation.PointToString();
                route.Waypoints = newRoute.Waypoints.PointsToString();
            }
            else // Create new
            {
                UnitOfWork.Routes.Add(new Data.Model.Route()
                {
                    StartLocation = newRoute.StartLocation.PointToString(),
                    EndLocation = newRoute.EndLocation.PointToString(),
                    Waypoints = newRoute.Waypoints.PointsToString(),
                    UserId = user.Id
                });
            }

            UnitOfWork.Complete();

            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete()
        {
            if (HostingEnvironment.IsDevelopment())
                System.Threading.Thread.Sleep(1000);

            var user = UnitOfWork.Users.CurrentUser(User);
            if (user == null)
                return HttpUnauthorized();

            var dbRoute = UnitOfWork.Routes.Find(x => x.UserId == user.Id).FirstOrDefault();
            if (dbRoute != null)
                UnitOfWork.Routes.Remove(dbRoute);

            UnitOfWork.Complete();

            return Ok();
        }
    }
}
