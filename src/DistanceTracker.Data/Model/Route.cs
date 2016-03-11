namespace DistanceTracker.Data.Model
{
    public class Route: Entity
    {
        public int UserId { get; set; }
        public string StartLocation { get; set; }
        public string EndLocation { get; set; }
        public string Waypoints { get; set; }
    }
}
