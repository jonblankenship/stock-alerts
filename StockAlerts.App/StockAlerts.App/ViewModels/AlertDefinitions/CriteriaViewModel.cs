using System;
using System.Windows.Input;
using Prism.Navigation;
using StockAlerts.App.Services.Logging;
using StockAlerts.App.ViewModels.Base;
using StockAlerts.Resources.Model;
using Xamarin.Forms;

namespace StockAlerts.App.ViewModels.AlertDefinitions
{
    public class CriteriaViewModel : ViewModelBase
    {
        private readonly AlertCriteria _alertCriteria;

        public CriteriaViewModel(
            AlertCriteria alertCriteria,
            INavigationService navigationService, 
            ILogger logger) : base(navigationService, logger)
        {
            _alertCriteria = alertCriteria ?? throw new ArgumentNullException(nameof(alertCriteria));
        }

        public ICommand RemoveCriteriaCommand => new Command(ExecuteRemoveCriteria);

        private void ExecuteRemoveCriteria(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
