using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using StockAlerts.Domain.Settings;
using Xunit;

namespace StockAlerts.Domain.Tests.Unit.Settings
{
    public class AppSettingsTests
    {
        [Fact]
        public void MarketOpenUtc_MarketOpenContainsTime_MarketOpenTodayReturned()
        {
            // Arrange
            var settings = new AppSettings
            {
                MarketOpenTime = "14:30"
            };

            // Act
            var result = settings.MarketOpenUtc;

            // Assert
            result.Should().Be(new DateTimeOffset(
                DateTime.UtcNow.Year,
                DateTime.UtcNow.Month,
                DateTime.UtcNow.Day,
                14,
                30,
                0,
                TimeSpan.Zero));
        }
        [Fact]
        public void MarketOpenUtc_MarketCloseContainsTime_MarketCloseTodayReturned()
        {
            // Arrange
            var settings = new AppSettings
            {
                MarketCloseTime = "21:00"
            };

            // Act
            var result = settings.MarketCloseUtc;

            // Assert
            result.Should().Be(new DateTimeOffset(
                DateTime.UtcNow.Year,
                DateTime.UtcNow.Month,
                DateTime.UtcNow.Day,
                21,
                0,
                0,
                TimeSpan.Zero));
        }
    }
}
