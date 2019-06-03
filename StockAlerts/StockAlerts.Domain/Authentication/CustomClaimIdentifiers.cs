namespace StockAlerts.Domain.Authentication
{
    public static class CustomClaimIdentifiers
    {
        public const string AppUserId = "appUserId";
        public const string Rol = "rol", Id = "id";
    }

    public static class CustomClaims
    {
        public const string ApiAccess = "api_access";
    }
}
