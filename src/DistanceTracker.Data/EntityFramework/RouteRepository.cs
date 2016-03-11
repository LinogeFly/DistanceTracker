using DistanceTracker.Data.Model;
using DistanceTracker.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DistanceTracker.Data.EntityFramework
{
    public class RouteRepository : Repository<Route>, IRouteRepository
    {
        public RouteRepository(DataContext context) : base(context)
        {
        }
    }
}
