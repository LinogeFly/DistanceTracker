using System;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Authorization;
using DistanceTracker.Data.UnitOfWork;
using DistanceTracker.Data.Model;

namespace DistanceTracker.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class ProgressController : Controller, IApiController
    {
        [FromServices]
        public IHostingEnvironment HostingEnvironment { get; set; }

        [FromServices]
        public IUnitOfWork UnitOfWork { get; set; }

        public ProgressController()
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

            return Ok(user.Progress);
        }

        [HttpPost]
        public IActionResult Post([FromBody]int distance)
        {
            if (HostingEnvironment.IsDevelopment())
                System.Threading.Thread.Sleep(1000);

            var user = UnitOfWork.Users.CurrentUser(User);
            if (user == null)
                return HttpUnauthorized();

            // Update progress
            user.Progress += distance;

            // Update history
            UnitOfWork.History.Add(new History()
            {
                Date = DateTime.Now,
                Distance = distance,
                UserId = user.Id
            });

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

            // Remove progress
            user.Progress = 0;

            // Remove history
            var history = UnitOfWork.History.Find(x => x.UserId == user.Id);
            UnitOfWork.History.RemoveRange(history);

            UnitOfWork.Complete();

            return Ok();
        }
    }
}
