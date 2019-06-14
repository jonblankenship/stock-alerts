using System;
using System.Threading.Tasks;
using System.Windows.Input;
using StockAlerts.Forms.Models.User;
using StockAlerts.Forms.Services.Account;
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

        public LoginViewModel(
            ISettingsService settingsService,
            IAccountService accountService)
        {
            _settingsService = settingsService;
            _accountService = accountService;

            _userName = new ValidatableObject<string>();
            _password = new ValidatableObject<string>();

            IsRegistering = false;

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
            IsRegistering = false;
        }

        private void SwitchToRegister()
        {
            IsRegistering = true;
        }

        private async Task SignInAsync()
        {
            IsBusy = true;

            var loginResult = await _accountService.LoginAsync(_userName.Value, _password.Value);

            if (!string.IsNullOrWhiteSpace(loginResult.AccessToken.Token))
            {
                _settingsService.AuthAccessToken = loginResult.AccessToken.Token;
                await NavigationService.NavigateToAsync<MainViewModel>();
                await NavigationService.RemoveLastFromBackStackAsync();
            }

            //LoginUrl = _identityService.CreateAuthorizationRequest();

            IsValid = true;
            IsLogin = true;
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
