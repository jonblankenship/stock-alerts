using Prism.Navigation;
using StockAlerts.App.Extensions;
using StockAlerts.App.Services.Logging;
using StockAlerts.App.ViewModels.Base;
using StockAlerts.Core.Enums;
using StockAlerts.Resources.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using StockAlerts.Core.Extensions;
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

            InitAlertCriteria();
        }

        public Guid AlertCriteriaId => _alertCriteria.AlertCriteriaId;

        public string SelectedType
        {
            get => _alertCriteria.Type?.ToDisplayString();
            set
            {
                _alertCriteria.Type = value.ToCriteriaType();
                RaisePropertyChanged(nameof(SelectedType));
                ErrorMessage = string.Empty;
                IsInvalid = false;
            }
        }

        public List<string> Types { get; } = new List<string>
        {
            CriteriaType.Price.ToDisplayString(),
            CriteriaType.DailyPercentageGainLoss.ToDisplayString()
        };

        public string SelectedOperator
        {
            get => _alertCriteria.Operator?.ToDisplayString();
            set
            {
                _alertCriteria.Operator = value.ToCriteriaOperator();
                RaisePropertyChanged(nameof(SelectedOperator));
                ErrorMessage = string.Empty;
                IsInvalid = false;
            }
        }

        public ObservableCollection<string> Operators { get; } = new ObservableCollection<string>();

        public decimal? Level
        {
            get => _alertCriteria.Level;
            set
            {
                _alertCriteria.Level = value;
                RaisePropertyChanged(nameof(Level));
                ErrorMessage = string.Empty;
                IsInvalid = false;
            }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        private bool _isInvalid;
        public bool IsInvalid
        {
            get => _isInvalid;
            set => SetProperty(ref _isInvalid, value);
        }

        public EventHandler RemoveCriteria;

        public ICommand RemoveCriteriaCommand => new Command(ExecuteRemoveCriteria);
        
        private void ExecuteRemoveCriteria(object obj)
        {
            RemoveCriteria?.Invoke(this, null);
        }
        
        private void InitAlertCriteria()
        {
            if (_alertCriteria.AlertCriteriaId == Guid.Empty)
            {
                _alertCriteria.Type = CriteriaType.Price;
            }

            SetOperators();
        }

        private void SetOperators()
        {
            Operators.Clear();
            switch (_alertCriteria.Type)
            {
                case CriteriaType.Price:
                case CriteriaType.DailyPercentageGainLoss:
                {
                    Operators.Add(CriteriaOperator.GreaterThan.ToDisplayString());
                    Operators.Add(CriteriaOperator.GreaterThanOrEqualTo.ToDisplayString());
                    Operators.Add(CriteriaOperator.LessThan.ToDisplayString());
                    Operators.Add(CriteriaOperator.LessThanOrEqualTo.ToDisplayString());
                    break;
                }
            }
        }

        public bool Validate()
        {
            ErrorMessage = string.Empty;

            if (!_alertCriteria.Level.HasValue || !_alertCriteria.Type.HasValue || !_alertCriteria.Operator.HasValue)
            {
                ErrorMessage = "Criteria incomplete.";
                IsInvalid = true;
                return false;
            }

            IsInvalid = false;
            return !IsInvalid;
        }

        public void AddToAlertCriteria(AlertCriteria alertCriteria)
        {
            alertCriteria.ChildrenCriteria.Add(_alertCriteria);
        }
    }
}
