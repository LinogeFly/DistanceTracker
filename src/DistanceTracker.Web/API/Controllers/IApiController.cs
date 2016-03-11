using DistanceTracker.Data.UnitOfWork;
using Microsoft.AspNet.Hosting;

namespace DistanceTracker.API.Controllers
{
    public interface IApiController
    {
        IHostingEnvironment HostingEnvironment { get; set; }
        IUnitOfWork UnitOfWork { get; set; }
    }
}
