using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Navigation;
using StockAlerts.App.Constants;
using StockAlerts.App.Services.Logging;
using StockAlerts.App.ViewModels.Base;
using StockAlerts.Resources.Model;
using Xamarin.Forms;

namespace StockAlerts.App.ViewModels.AlertDefinitions
{
    public class CreateAlertDefinitionPageViewModel : ViewModelBase
    {
        private Stock _stock;

        public CreateAlertDefinitionPageViewModel(
            INavigationService navigationService,
            ILogger logger) : base(navigationService, logger)
        {
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters != null && parameters.ContainsKey(NavigationParameterKeys.SelectedStock))
            {
                _stock = (Stock)parameters[NavigationParameterKeys.SelectedStock];
                RaisePropertyChanged(nameof(StockSymbol));
                AlertName = $"{_stock.Symbol} Alert";
            }
        }

        public string StockSymbol => _stock?.Symbol;

        private string _alertName;

        public string AlertName
        {
            get => _alertName;
            set => SetProperty(ref _alertName, value);
        }

        public ObservableCollection<CriteriaViewModel> CriteriaCollection { get; set; } = new ObservableCollection<CriteriaViewModel>();

        public ICommand AddCriteriaCommand => new Command(ExecuteAddCriteria);

        private void ExecuteAddCriteria()
        {
            CriteriaCollection.Add(new CriteriaViewModel(new AlertCriteria(), NavigationService, Logger));
        }

        public ICommand SaveCommand => new Command(async () => await ExecuteSaveAsync());

        private async Task ExecuteSaveAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}
