namespace StockAlerts.Domain.Model
{
    public interface IStockInfo
    {
        string Ticker { get; set; }

        string SecurityName { get; set; }
    }
}
