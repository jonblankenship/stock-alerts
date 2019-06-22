using System;

namespace StockAlerts.App.Services.Logging
{
    public class ConsoleLogger : ILogger
    {
        public void Log(string message)
            => Console.Out.WriteLine(message);
    }
}
