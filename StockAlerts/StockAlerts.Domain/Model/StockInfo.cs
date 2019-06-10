namespace StockAlerts.Domain.Model
{
    public class StockInfo
    {
        public StockInfo(IStockInfo stockInfo)
        {
            Ticker = stockInfo.Ticker;
            SecurityName = stockInfo.SecurityName;
        }

        public string Ticker { get; set; }

        public string SecurityName { get; set; }
    }
}
