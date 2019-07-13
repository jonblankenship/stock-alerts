using StockAlerts.Core.Enums;
using System.Collections.Generic;
using System.Linq;

namespace StockAlerts.App.Extensions
{
    public static class CriteriaTypeExtensions
    {
        private static readonly Dictionary<CriteriaType, string> _criteriaTypeStringMatrix = new Dictionary<CriteriaType, string>
        {
            { CriteriaType.Price, "Price" },
            { CriteriaType.DailyPercentageGainLoss, "Daily % Change" }
        };

        public static string ToDisplayString(this CriteriaType criteriaType)
        {
            if (_criteriaTypeStringMatrix.ContainsKey(criteriaType))
                return _criteriaTypeStringMatrix[criteriaType];

            return string.Empty;
        }

        public static CriteriaType ToCriteriaType(this string str)
        {
            return _criteriaTypeStringMatrix.SingleOrDefault(x => x.Value == str).Key;
        }
    }
}
