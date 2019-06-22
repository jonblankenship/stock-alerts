using Prism.Navigation;
using StockAlerts.App.Services.Logging;
using StockAlerts.App.ViewModels.Base;

namespace StockAlerts.App.ViewModels.AlertDefinitions
{
    public class CreateAlertDefinitionViewModel : ViewModelBase
    {
        public CreateAlertDefinitionViewModel(
            INavigationService navigationService,
            ILogger logger) : base(navigationService, logger)
        {

        }


    }
}
