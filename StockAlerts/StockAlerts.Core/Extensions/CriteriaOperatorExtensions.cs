using StockAlerts.Core.Enums;
using System.Collections.Generic;
using System.Linq;

namespace StockAlerts.Core.Extensions
{
    public static class CriteriaOperatorExtensions
    {
        private static readonly Dictionary<CriteriaOperator, string> _criteriaOperatorStringMatrix = new Dictionary<CriteriaOperator, string>
        {
            { CriteriaOperator.GreaterThan, ">" },
            { CriteriaOperator.GreaterThanOrEqualTo, ">=" },
            { CriteriaOperator.LessThan, "<" },
            { CriteriaOperator.LessThanOrEqualTo, "<=" },
            { CriteriaOperator.Equals, "=" }
        };

        public static string ToDisplayString(this CriteriaOperator criteriaType)
        {
            if (_criteriaOperatorStringMatrix.ContainsKey(criteriaType))
                return _criteriaOperatorStringMatrix[criteriaType];

            return string.Empty;
        }

        public static CriteriaOperator ToCriteriaOperator(this string str)
        {
            return _criteriaOperatorStringMatrix.SingleOrDefault(x => x.Value == str).Key;
        }
    }
}
