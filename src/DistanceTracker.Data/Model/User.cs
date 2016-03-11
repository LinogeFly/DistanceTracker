namespace DistanceTracker.Data.Model
{
    public class User : Entity
    {
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public long Progress { get; set; }
    }
}
