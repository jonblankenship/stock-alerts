using System;
using System.Threading.Tasks;
using System.Windows.Input;
using StockAlerts.Domain.Authentication;
using StockAlerts.Forms.Models.User;
using StockAlerts.Forms.Services.Account;
using StockAlerts.Forms.Services.RequestProvider;
using StockAlerts.Forms.Services.Settings;
using StockAlerts.Forms.Validations;
using StockAlerts.Forms.ViewModels.Base;
using Xamarin.Forms;

namespace StockAlerts.Forms.ViewModels
{
    public class LoginViewModel : ViewModelBase
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

        public LoginViewModel(
            ISettingsService settingsService,
            IAccountService accountService)
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
            get
            {
                return _userName;
            }
            set
            {
                _userName = value;
                _userName.Validate();
                LoginErrorMessage = string.Empty;
                RaisePropertyChanged(() => UserName);
            }
        }

        public ValidatableObject<string> EmailAddress
        {
            get
            {
                return _emailAddress;
            }
            set
            {
                _emailAddress = value;
                _emailAddress.Validate();
                LoginErrorMessage = string.Empty;
                RaisePropertyChanged(() => EmailAddress);
            }
        }

        public ValidatableObject<string> Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
                _password.Validate();
                LoginErrorMessage = string.Empty;
                RaisePropertyChanged(() => Password);
            }
        }

        public bool IsValid
        {
            get
            {
                return _isValid;
            }
            set
            {
                _isValid = value;
                RaisePropertyChanged(() => IsValid);
            }
        }

        public bool IsRegistering
        {
            get { return _isRegistering; }
            set
            {
                _isRegistering = value;
                RaisePropertyChanged(() => IsRegistering);
                RaisePropertyChanged(() => LoginButtonText);
            }
        }

        public bool IsLogin
        {
            get
            {
                return _isLogin;
            }
            set
            {
                _isLogin = value;
                RaisePropertyChanged(() => IsLogin);
            }
        }

        public string LoginUrl
        {
            get
            {
                return _authUrl;
            }
            set
            {
                _authUrl = value;
                RaisePropertyChanged(() => LoginUrl);
            }
        }

        public string LoginErrorMessage
        {
            get { return _loginErrorMessage; }
            set
            {
                _loginErrorMessage = value;
                RaisePropertyChanged(() => LoginErrorMessage);
            }
        }

        public string LoginButtonText
        {
            get => IsRegistering ? "[ REGISTER ]" : "[ LOGIN ]";
        }

        public ICommand SwitchToLoginCommand => new Command(SwitchToLogin);

        public ICommand SwitchToRegisterCommand => new Command(SwitchToRegister);

        public ICommand SignInCommand => new Command(async () => await SignInAsync());

        public ICommand RegisterCommand => new Command(Register);
        
        public ICommand SettingsCommand => new Command(async () => await SettingsAsync());

        public ICommand ValidateEmailAddressCommand => new Command(() => ValidateEmailAddress());

        public ICommand ValidateUserNameCommand => new Command(() => ValidateUserName());

        public ICommand ValidatePasswordCommand => new Command(() => ValidatePassword());

        public override Task InitializeAsync(object navigationData)
        {
            if (navigationData is LogoutParameter)
            {
                var logoutParameter = (LogoutParameter)navigationData;

                if (logoutParameter.Logout)
                {
                    Logout();
                }
            }

            return base.InitializeAsync(navigationData);
        }

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
                    await NavigationService.NavigateToAsync<MainViewModel>();
                    await NavigationService.RemoveLastFromBackStackAsync();
                }
            }

            //LoginUrl = _identityService.CreateAuthorizationRequest();
            //IsValid = true;
            //IsLogin = true;

            IsBusy = false;
        }

        private void Register()
        {
            //_openUrlService.OpenUrl(GlobalSetting.Instance.RegisterWebsite);
        }

        private void Logout()
        {
            //var authIdToken = _settingsService.AuthIdToken;
            //var logoutRequest = _identityService.CreateLogoutRequest(authIdToken);

            //if (!string.IsNullOrEmpty(logoutRequest))
            //{
            //    // Logout
            //    LoginUrl = logoutRequest;
            //}
        }

        private async Task SettingsAsync()
        {
            await NavigationService.NavigateToAsync<SettingsViewModel>();
        }

        private bool Validate()
        {
            bool isValidEmailAddress = !IsRegistering || ValidateEmailAddress();
            bool isValidUser = ValidateUserName();
            bool isValidPassword = ValidatePassword();

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
