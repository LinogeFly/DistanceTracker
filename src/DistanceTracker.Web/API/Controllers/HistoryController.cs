using System;
using System.Linq;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Authorization;
using DistanceTracker.Data.UnitOfWork;

namespace DistanceTracker.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class HistoryController : Controller, IApiController
    {
        [FromServices]
        public IHostingEnvironment HostingEnvironment { get; set; }

        [FromServices]
        public IUnitOfWork UnitOfWork { get; set; }

        public HistoryController()
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

            var history = UnitOfWork.History
                .Find(x => x.UserId == user.Id)
                .OrderByDescending(x => x.Date)
                .Take(15)
                .ToArray();

            return Ok(history);
        }
    }
}
