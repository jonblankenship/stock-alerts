using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Navigation;
using StockAlerts.App.Services.AlertDefinitions;
using StockAlerts.App.Services.Logging;
using StockAlerts.App.ViewModels.Base;
using StockAlerts.App.Views.AlertDefinitions;
using Xamarin.Forms;

namespace StockAlerts.App.ViewModels.AlertDefinitions
{
    public class AlertsPageViewModel : ViewModelBase
    {
        private readonly IAlertDefinitionsService _alertDefinitionsService;
        private ObservableCollection<AlertDefinitionItemViewModel> _alertDefinitions;

        public AlertsPageViewModel(
            IAlertDefinitionsService alertDefinitionsService,
            INavigationService navigationService,
            ILogger logger) : base(navigationService, logger)
        {
            _alertDefinitionsService = alertDefinitionsService ?? throw new ArgumentNullException(nameof(alertDefinitionsService));
        }

        public ObservableCollection<AlertDefinitionItemViewModel> AlertDefinitions
        {
            get => _alertDefinitions;
            set
            {
                SetProperty(ref _alertDefinitions, value);
            }
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            IsBusy = true;

            // Get alert definitions
            try
            {
                AlertDefinitions = await _alertDefinitionsService.GetAlertDefinitionItemViewModelsAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            IsBusy = false;
        }

        public ICommand CreateAlertCommand => new Command(ExecuteCreateAlert);

        private void ExecuteCreateAlert()
        {
            NavigationService.NavigateAsync(nameof(CreateAlertDefinitionPage));
        }
    }
}
