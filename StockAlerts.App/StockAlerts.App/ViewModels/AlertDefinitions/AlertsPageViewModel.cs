using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Navigation;
using StockAlerts.App.Constants;
using StockAlerts.App.Services.AlertDefinitions;
using StockAlerts.App.Services.Logging;
using StockAlerts.App.ViewModels.Base;
using StockAlerts.App.Views.AlertDefinitions;
using StockAlerts.Resources.Model;
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

            IsBusy = true;
        }

        public ObservableCollection<AlertDefinitionItemViewModel> AlertDefinitions
        {
            get => _alertDefinitions;
            set => SetProperty(ref _alertDefinitions, value);
        }

        private AlertDefinitionItemViewModel _selectedAlertDefinition;
        public AlertDefinitionItemViewModel SelectedAlertDefinition
        {
            get => _selectedAlertDefinition;
            set
            {
                if (value != null)
                {
                    var navigationParams = new NavigationParameters();
                    navigationParams.Add(NavigationParameterKeys.SelectedAlertDefinition, value);
                    NavigationService.NavigateAsync(nameof(EditAlertDefinitionPage), navigationParams);
                }
            }
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
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
            NavigationService.NavigateAsync(nameof(EditAlertDefinitionPage));
        }
    }
}
