using FluentAssertions;
using StockAlerts.Domain.Model;
using System;
using Xunit;

namespace StockAlerts.Domain.Tests.Unit.Model
{
    public class StockTests
    {
        [Fact]
        public void OnNewQuote_SymbolDoesNotMatch_ApplicationException()
        {
            // Arrange
            var stock = new Stock {Symbol = "ABC"};
            var quote = new PriceQuote()
            {
                LastPrice = 9.99M,
                LastTime = DateTimeOffset.Now.AddMinutes(-123),
                Symbol = "XYZ"
            };

            // Act
            Assert.Throws<ApplicationException>(() => stock.OnNewQuote(quote));
        }

        [Fact]
        public void OnNewQuote_SymbolMatches_StockUpdatedWithQuote()
        {
            // Arrange
            var stock = new Stock
            {
                Symbol = "ABC",
                LastPrice = 9.56M
            };
            var quote = new PriceQuote
            {
                LastPrice = 9.99M,
                LastTime = DateTimeOffset.Now.AddMinutes(-123),
                Symbol = "ABC"
            };

            // Act
            stock.OnNewQuote(quote);

            // Assert
            stock.LastPrice.Should().Be(quote.LastPrice);
            stock.PreviousLastPrice.Should().Be(9.56M);
            stock.LastTime.Should().Be(quote.LastTime);
            stock.Symbol.Should().Be(quote.Symbol);
        }
    }
}
