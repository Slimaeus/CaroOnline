namespace Utility.Constants;

public static class SystemConstants
{
    public const string ConnectionStringKey = "CaroDatabase";
    public static class AppSettings
    {
        public const string Token = "Token";
        public const string BaseAddress = "CaroAPIBaseUrl";
        public const string GameConnectionStringKey = "GameSqliteDb";
    }
    public static class Game
    {
        public const int WinScore = 20;
        public const int LoseScore = 10;
        public const int DrawScore = 5;
    }
}