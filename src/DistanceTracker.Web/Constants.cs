namespace DistanceTracker
{
    static class Constants
    {
        public const int AuthExpireDays = 3650; // 10 years

        public static class Cookies
        {
            public const string AuthenticationToken = "AuthToken";
        }

        public static class Settings
        {
            public static class Keys
            {
                public const string SQLiteFilePath = "Database:SQLiteFilePath";
            }
        }
    }
}
