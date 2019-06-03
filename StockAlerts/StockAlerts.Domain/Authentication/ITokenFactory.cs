namespace StockAlerts.Domain.Authentication
{
    public interface ITokenFactory
    {
        string GenerateToken(int size = 32);
    }
}
