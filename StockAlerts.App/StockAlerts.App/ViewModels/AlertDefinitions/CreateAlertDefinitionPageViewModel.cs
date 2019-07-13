using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Navigation;
using StockAlerts.App.Constants;
using StockAlerts.App.Services.AlertDefinitions;
using StockAlerts.App.Services.Logging;
using StockAlerts.App.ViewModels.Base;
using StockAlerts.Core.Enums;
using StockAlerts.Resources.Model;
using Xamarin.Forms;

namespace StockAlerts.App.ViewModels.AlertDefinitions
{
    public class CreateAlertDefinitionPageViewModel : ViewModelBase
    {
        private readonly IAlertDefinitionsService _alertDefinitionsService;
        private Stock _stock;
        private AlertDefinition _alertDefinition;

        public CreateAlertDefinitionPageViewModel(
            IAlertDefinitionsService alertDefinitionsService,
            INavigationService navigationService,
            ILogger logger) : base(navigationService, logger)
        {
            _alertDefinitionsService = alertDefinitionsService ?? throw new ArgumentNullException(nameof(alertDefinitionsService));
            _alertDefinition = new AlertDefinition();
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

        public bool IsAndOperator { get; set; }

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
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
            var isValid = Validate();

            if (isValid)
            {
                _alertDefinition.StockId = _stock.StockId;
                _alertDefinition.Name = _alertName;
                _alertDefinition.Status = AlertDefinitionStatuses.Enabled;

                _alertDefinition.RootCriteria = new AlertCriteria
                {
                    Type = CriteriaType.Composite,
                    Operator = IsAndOperator ? CriteriaOperator.And : CriteriaOperator.Or
                };

                foreach (var c in CriteriaCollection)
                {
                    c.AddToAlertCriteria(_alertDefinition.RootCriteria);
                }

                await _alertDefinitionsService.SaveAlertDefinitionAsync(_alertDefinition);
            }
        }

        private bool Validate()
        {
            var messages = new List<string>();
            if (string.IsNullOrWhiteSpace(AlertName))
                messages.Add("Alert name must be specified.");

            if (string.IsNullOrWhiteSpace(StockSymbol))
                messages.Add("A stock must be selected.");

            if (!CriteriaCollection.Any())
                messages.Add("At least one criteria must be specified.");

            var areCriteriaValid = true;
            foreach (var c in CriteriaCollection)
            {
                if (!c.Validate())
                    areCriteriaValid = false;
            }

            if (!areCriteriaValid)
                messages.Add("Criteria incomplete.");

            if (messages.Any())
            {
                ErrorMessage = string.Join(Environment.NewLine, messages);
                return false;
            }
            
            return true;    
        }
    }
}

