using System;
using System.Linq;

namespace StockAlerts.Domain.Settings
{
    public class AppSettings : IAppSettings
    {
        public string MarketOpenTime { get; set; }

        public string MarketCloseTime { get; set; }

        public DateTimeOffset MarketOpenUtc => ConvertToTimeToday(MarketOpenTime);

        public DateTimeOffset MarketCloseUtc => ConvertToTimeToday(MarketCloseTime);

        private DateTimeOffset ConvertToTimeToday(string time)
        {
            var now = DateTime.UtcNow;
            var timeParts = time.Split(':');
            return new DateTimeOffset(
                now.Year,
                now.Month,
                now.Day,
                int.Parse(timeParts.First()),
                int.Parse(timeParts.Last()),
                0,
                TimeSpan.Zero);
        }
    }
}
