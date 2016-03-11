using System;

namespace DistanceTracker.Data.Model
{
    public class History : Entity
    {
        public int UserId { get; set; }
        public int Distance { get; set; }
        public DateTime Date { get; set; }
    }
}
