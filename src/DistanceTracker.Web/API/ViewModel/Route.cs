using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DistanceTracker.API.ViewModel
{
    public class Route
    {
        public Point StartLocation { get; set; }
        public Point EndLocation { get; set; }
        public IEnumerable<Point> Waypoints { get; set; }
    }
}
