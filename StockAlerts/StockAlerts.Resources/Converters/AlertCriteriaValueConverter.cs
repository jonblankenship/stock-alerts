using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using StockAlerts.Resources.Model;

namespace StockAlerts.Resources.Converters
{
    public class AlertCriteriaValueConverter : IValueConverter<AlertCriteria, ICollection<Domain.Model.AlertCriteria>>
    {
        public ICollection<Domain.Model.AlertCriteria> Convert(AlertCriteria sourceMember, ResolutionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
