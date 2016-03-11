using Newtonsoft.Json;
using System.Collections.Generic;

namespace DistanceTracker.API.ViewModel
{
    public static class ViewModelExtensions
    {
        public static string PointToString(this Point point)
        {
            return JsonConvert.SerializeObject(point);
        }

        public static Point ToPoint(this string str)
        {
            return JsonConvert.DeserializeObject<Point>(str);
        }

        public static IEnumerable<Point> ToPoints(this string str)
        {
            return JsonConvert.DeserializeObject<IEnumerable<Point>>(str);
        }

        public static string PointsToString(this IEnumerable<Point> points)
        {
            return JsonConvert.SerializeObject(points);
        }
    }
}
