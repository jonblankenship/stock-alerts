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
using StockAlerts.App.Views.AlertDefinitions;
using StockAlerts.Core.Enums;
using StockAlerts.Resources.Model;
using Xamarin.Forms;

namespace StockAlerts.App.ViewModels.AlertDefinitions
{
    public class EditAlertDefinitionPageViewModel : ViewModelBase
    {
        private readonly IAlertDefinitionsService _alertDefinitionsService;
        private Stock _stock;
        private AlertDefinition _alertDefinition;

        public EditAlertDefinitionPageViewModel(
            IAlertDefinitionsService alertDefinitionsService,
            INavigationService navigationService,
            ILogger logger) : base(navigationService, logger)
        {
            _alertDefinitionsService = alertDefinitionsService ?? throw new ArgumentNullException(nameof(alertDefinitionsService));
            _alertDefinition = new AlertDefinition
            {
                RootCriteria = new AlertCriteria
                {
                    Type = CriteriaType.Composite,
                    Operator = CriteriaOperator.And
                }
            };

            Title = "Alert Definition";
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters != null)
            {
                if (parameters.ContainsKey(NavigationParameterKeys.SelectedStock))
                {
                    // If parameters contains NavigationParameterKeys.SelectedStock, then we're navigating back from stock search page
                    _stock = (Stock)parameters[NavigationParameterKeys.SelectedStock];
                    RaisePropertyChanged(nameof(StockSymbol));
                    AlertName = $"{_stock.Symbol} Alert";
                }
                else if (parameters.ContainsKey((NavigationParameterKeys.SelectedAlertDefinition)))
                {
                    // If parameters contains NavigationParameterKeys.SelectedAlertDefinition, then we're navigating from the alert defs page
                    var alertDefinitionItemViewModel = (AlertDefinitionItemViewModel) parameters[NavigationParameterKeys.SelectedAlertDefinition];
                    _alertDefinition = alertDefinitionItemViewModel.AlertDefinition;
                    InitializeForEdit();
                }
            }
        }

        public string StockSymbol => _stock?.Symbol;

        private string _alertName;
        public string AlertName
        {
            get => _alertName;
            set => SetProperty(ref _alertName, value);
        }


        public int SelectedOperatorButtonIndex
        {
            get => (int)(_alertDefinition.RootCriteria?.Operator ?? CriteriaOperator.And);
            set => _alertDefinition.RootCriteria.Operator = (CriteriaOperator)Enum.ToObject(typeof(CriteriaOperator), value);
        }

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
            AddCriteria(new CriteriaViewModel(new AlertCriteria(), NavigationService, Logger));
        }

        private void AddCriteria(CriteriaViewModel criteriaViewModel)
        {
            CriteriaCollection.Add(criteriaViewModel);

            criteriaViewModel.RemoveCriteria += RemoveCriteria;
        }

        private void RemoveCriteria(object sender, EventArgs e)
        {
            var criteriaViewModel = sender as CriteriaViewModel;
            CriteriaCollection.Remove(criteriaViewModel);
            criteriaViewModel.RemoveCriteria -= RemoveCriteria;
        }

        public ICommand SaveCommand => new Command(async () => await ExecuteSaveAsync());

        private async Task ExecuteSaveAsync()
        {
            IsBusy = true;

            var isValid = Validate();

            if (isValid)
            {
                _alertDefinition.StockId = _stock.StockId;
                _alertDefinition.Name = _alertName;
                _alertDefinition.Status = AlertDefinitionStatuses.Enabled;

                if (_alertDefinition.RootCriteria.ChildrenCriteria == null)
                    _alertDefinition.RootCriteria.ChildrenCriteria = new List<AlertCriteria>();

                foreach (var c in CriteriaCollection)
                {
                    if (!_alertDefinition.RootCriteria.ChildrenCriteria.Select(x => x.AlertCriteriaId).Contains(c.AlertCriteriaId))
                        c.AddToAlertCriteria(_alertDefinition.RootCriteria);
                }

                await _alertDefinitionsService.SaveAlertDefinitionAsync(_alertDefinition);

                await NavigationService.NavigateAsync(nameof(AlertsPage));
            }

            IsBusy = false;
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

        private void InitializeForEdit()
        {
            Title = "Edit Alert Definition";
            _stock = _alertDefinition.Stock;
            AlertName = _alertDefinition.Name;
            if (_alertDefinition.RootCriteria != null)
            {
                //SelectedOperatorButtonIndex = _alertDefinition.RootCriteria?.Operator == CriteriaOperator.Or ? 1 : 0;
                foreach (var c in _alertDefinition.RootCriteria?.ChildrenCriteria)
                {
                    AddCriteria(new CriteriaViewModel(c, NavigationService, Logger));
                }
            }
            
            RaisePropertyChanged(nameof(StockSymbol));
            RaisePropertyChanged(nameof(SelectedOperatorButtonIndex));
        }
    }
}

