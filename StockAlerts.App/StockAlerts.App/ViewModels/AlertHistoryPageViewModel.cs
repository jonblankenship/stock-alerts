using System;
using System.Collections.Generic;
using System.Text;
using Prism.Navigation;
using StockAlerts.App.Services.Logging;
using StockAlerts.App.ViewModels.Base;

namespace StockAlerts.App.ViewModels
{
    public class AlertHistoryPageViewModel : ViewModelBase
    {
        public AlertHistoryPageViewModel(INavigationService navigationService, ILogger logger) : base(navigationService, logger)
        {
        }
    }
}
