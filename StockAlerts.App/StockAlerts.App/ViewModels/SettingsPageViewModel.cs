using Prism.Navigation;
using StockAlerts.App.Services.Logging;
using StockAlerts.App.Services.Settings;
using StockAlerts.App.Services.UserPreferences;
using StockAlerts.App.ViewModels.Base;
using StockAlerts.Resources.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace StockAlerts.App.ViewModels
{
    public class SettingsPageViewModel : ViewModelBase
    {
        private readonly IUserPreferencesService _userPreferencesService;

        private UserPreferences _userPreferences;

        public SettingsPageViewModel(
            IUserPreferencesService userPreferencesService,
            INavigationService navigationService,
            ILogger logger) : base(navigationService, logger)
        {
            _userPreferencesService = userPreferencesService ?? throw new ArgumentNullException(nameof(userPreferencesService));

            Title = "User Settings";

            IsActiveChanged += OnIsActiveChanged;
        }

        public bool ShouldSendPush
        {
            get => _userPreferences?.ShouldSendPush ?? false;
            set
            {
                if (_userPreferences != null)
                {
                    _userPreferences.ShouldSendPush = value;
                    ErrorMessage = string.Empty;
                    RaisePropertyChanged();
                }
            }
        }

        public bool ShouldSendEmail
        {
            get => _userPreferences?.ShouldSendEmail ?? false;
            set
            {
                if (_userPreferences != null)
                {
                    _userPreferences.ShouldSendEmail = value;
                    ErrorMessage = string.Empty;
                    RaisePropertyChanged();
                }
            }
        }

        public bool ShouldSendSms
        {
            get => _userPreferences?.ShouldSendSms ?? false;
            set
            {
                if (_userPreferences != null)
                {
                    _userPreferences.ShouldSendSms = value;
                    ErrorMessage = string.Empty;
                    RaisePropertyChanged();
                }
            }
        }

        public string EmailAddress
        {
            get => _userPreferences?.EmailAddress;
            set
            {
                if (_userPreferences != null)
                {
                    _userPreferences.EmailAddress = value;
                    ErrorMessage = string.Empty;
                    RaisePropertyChanged();
                }
            }
        }

        public string SmsNumber
        {
            get => _userPreferences?.SmsNumber;
            set
            {
                if (_userPreferences != null)
                {
                    _userPreferences.SmsNumber =  value;
                    ErrorMessage = string.Empty;
                    RaisePropertyChanged();
                }
            }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public async void OnIsActiveChanged(object obj, EventArgs args)
        {
            if (((SettingsPageViewModel)obj).IsActive)
            {
                IsBusy = true;

                _userPreferences = await _userPreferencesService.GetUserPreferencesAsync();
                RaisePropertyChanged(nameof(ShouldSendPush));
                RaisePropertyChanged(nameof(ShouldSendEmail));
                RaisePropertyChanged(nameof(ShouldSendSms));
                RaisePropertyChanged(nameof(EmailAddress));
                RaisePropertyChanged(nameof(SmsNumber));

                IsBusy = false;
            }
        }

        public ICommand SaveCommand => new Command(async () => await ExecuteSaveAsync());

        private async Task ExecuteSaveAsync()
        {
            IsBusy = true;

            _userPreferences.SmsNumber = _userPreferences.SmsNumber.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");

            var isValid = Validate();

            if (isValid)
            {
                await _userPreferencesService.SaveUserPreferencesAsync(_userPreferences);
            }

            IsBusy = false;
        }

        private bool Validate()
        {
            var errorMessages = new List<string>();

            if (ShouldSendEmail)
            {
                if (string.IsNullOrWhiteSpace(EmailAddress))
                    errorMessages.Add("E-mail address is required.");
                else
                {
                    Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                    Match match = regex.Match(EmailAddress);
                    if (!match.Success)
                        errorMessages.Add("E-mail address is not valid.");
                }
            }

            if (ShouldSendSms)
            {
                if (string.IsNullOrWhiteSpace(SmsNumber))
                    errorMessages.Add("Phone number is required.");
                else if (SmsNumber.Length != 10)
                    errorMessages.Add("Phone number must be 10 digits.");
            }

            if (errorMessages.Any())
            {
                ErrorMessage = string.Join(Environment.NewLine, errorMessages);
                return false;
            }

            return true;
        }
    }
}
