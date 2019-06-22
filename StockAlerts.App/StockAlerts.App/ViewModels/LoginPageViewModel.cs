using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Navigation;
using StockAlerts.Domain.Authentication;
using StockAlerts.App.Models.User;
using StockAlerts.App.Services.Accounts;
using StockAlerts.App.Services.Logging;
using StockAlerts.App.Services.RequestProvider;
using StockAlerts.App.Services.Settings;
using StockAlerts.App.Validations;
using StockAlerts.App.ViewModels.Base;
using StockAlerts.App.Views;
using Xamarin.Forms;

namespace StockAlerts.App.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        private readonly ISettingsService _settingsService;
        private readonly IAccountService _accountService;
        private ValidatableObject<string> _emailAddress;
        private ValidatableObject<string> _userName;
        private ValidatableObject<string> _password;
        private bool _isValid;
        private bool _isRegistering;
        private bool _isLogin;
        private string _authUrl;
        private string _loginErrorMessage;

        public LoginPageViewModel(
            ISettingsService settingsService,
            IAccountService accountService,
            INavigationService navigationService,
            ILogger logger): base(navigationService, logger)
        {
            _settingsService = settingsService;
            _accountService = accountService;

            _emailAddress = new ValidatableObject<string>();
            _userName = new ValidatableObject<string>();
            _password = new ValidatableObject<string>();

            IsRegistering = false;

            _emailAddress.PropertyChanged += (sender, args) => { LoginErrorMessage = string.Empty; };
            _userName.PropertyChanged += (sender, args) => { LoginErrorMessage = string.Empty; };
            _password.PropertyChanged += (sender, args) => { LoginErrorMessage = string.Empty; };

            AddValidations();
        }

        public ValidatableObject<string> UserName
        {
            get => _userName;
            set
            {
                SetProperty(ref _userName, value);
                _userName.Validate();
                LoginErrorMessage = string.Empty;
            }
        }

        public ValidatableObject<string> EmailAddress
        {
            get => _emailAddress;
            set
            {
                SetProperty(ref _emailAddress, value);
                _emailAddress.Validate();
                LoginErrorMessage = string.Empty;
            }
        }

        public ValidatableObject<string> Password
        {
            get => _password;
            set
            {
                SetProperty(ref _password, value);
                _password.Validate();
                LoginErrorMessage = string.Empty;
            }
        }

        public bool IsValid
        {
            get => _isValid;
            set
            {
                SetProperty(ref _isValid, value);
            }
        }

        public bool IsRegistering
        {
            get { return _isRegistering; }
            set
            {
                SetProperty(ref _isRegistering, value);
                RaisePropertyChanged(nameof(LoginButtonText));
            }
        }

        public string LoginErrorMessage
        {
            get { return _loginErrorMessage; }
            set
            {
                SetProperty(ref _loginErrorMessage, value);
            }
        }

        public string LoginButtonText
        {
            get => IsRegistering ? "[ REGISTER ]" : "[ LOGIN ]";
        }

        public ICommand SwitchToLoginCommand => new Command(SwitchToLogin);

        public ICommand SwitchToRegisterCommand => new Command(SwitchToRegister);

        public ICommand SignInCommand => new Command(async () => await SignInAsync());

        private void SwitchToLogin()
        {
            if (IsRegistering)
            {
                ClearFormFields();
                IsRegistering = false;
            }
        }

        private void SwitchToRegister()
        {
            if (!IsRegistering)
            {
                ClearFormFields();
                IsRegistering = true;
            }
        }

        private void ClearFormFields()
        {
            _emailAddress.Value = string.Empty;
            _userName.Value = string.Empty;
            _password.Value = string.Empty;
            _emailAddress.ResetValidation();
            _userName.ResetValidation();
            _password.ResetValidation();
            LoginErrorMessage = string.Empty;
        }

        private async Task SignInAsync()
        {
            IsBusy = true;

            if (Validate())
            {
                LoginResponse loginResult = null;

                try
                {
                    if (IsRegistering)
                    {
                        loginResult = await _accountService.RegisterAsync(_emailAddress.Value, _userName.Value, _password.Value);
                    }
                    else
                    {
                        loginResult = await _accountService.LoginAsync(_userName.Value, _password.Value);
                    }
                }
                catch (HttpRequestExceptionEx e)
                {
                    if (e.Error != null)
                    {
                        LoginErrorMessage = e.Error.Error;
                        IsBusy = false;
                        return;
                    }

                    throw;
                }

                if (!string.IsNullOrWhiteSpace(loginResult.AccessToken.Token))
                {
                    _settingsService.AuthAccessToken = loginResult.AccessToken.Token;
                    await NavigationService.NavigateAsync(nameof(MainPage));
                }
            }

            IsBusy = false;
        }

        private bool Validate()
        {
            var isValidEmailAddress = !IsRegistering || ValidateEmailAddress();
            var isValidUser = ValidateUserName();
            var isValidPassword = ValidatePassword();

            return isValidUser && isValidPassword && isValidEmailAddress;
        }

        private bool ValidateUserName()
        {
            return _userName.Validate();
        }

        private bool ValidatePassword()
        {
            return _password.Validate();
        }

        private bool ValidateEmailAddress()
        {
            return _emailAddress.Validate();
        }

        private void AddValidations()
        {
            _emailAddress.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "An e-mail address is required." });
            _userName.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "A username is required." });
            _password.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "A password is required." });
        }
    }
}
