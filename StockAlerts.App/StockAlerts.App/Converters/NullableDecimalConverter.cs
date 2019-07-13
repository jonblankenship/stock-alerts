using System;
using System.Globalization;
using Xamarin.Forms;

namespace StockAlerts.App.Converters
{
    public class NullableDecimalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var nullable = value as decimal?;
            var result = string.Empty;

            if (nullable.HasValue)
            {
                result = nullable.Value.ToString();
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var stringValue = value as string;
            int decimalValue;
            decimal? result = null;

            if (int.TryParse(stringValue, out decimalValue))
            {
                result = new decimal?(decimalValue);
            }

            return result;
        }
    }
}
