using Prism.Navigation;
using StockAlerts.App.Services.Logging;
using StockAlerts.App.Services.Settings;
using StockAlerts.App.Services.UserPreferences;
using StockAlerts.App.ViewModels.Base;
using StockAlerts.Resources.Model;
using System;
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
                    RaisePropertyChanged(nameof(ShouldSendPush));
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
                    RaisePropertyChanged(nameof(ShouldSendEmail));
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
                    RaisePropertyChanged(nameof(ShouldSendSms));
                }
            }
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

                IsBusy = false;
            }
        }

        public ICommand SaveCommand => new Command(async () => await ExecuteSaveAsync());

        private async Task ExecuteSaveAsync()
        {
            IsBusy = true;

            var isValid = Validate();

            if (isValid)
            {
                await _userPreferencesService.SaveUserPreferencesAsync(_userPreferences);
            }

            IsBusy = false;
        }

        private bool Validate()
        {
            return true;
        }
    }
}
