namespace DistanceTracker.Data.Model
{
    public class Entity : IEntity
    {
        public int Id { get; set; }
    }

    public interface IEntity
    {
        int Id { get; set; }
    }
}
