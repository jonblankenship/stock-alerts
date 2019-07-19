using System;
using System.Threading.Tasks;
using Prism;
using Prism.AppModel;
using Prism.Mvvm;
using Prism.Navigation;
using StockAlerts.App.Services.Logging;

namespace StockAlerts.App.ViewModels.Base
{
 
    public class ViewModelBase : BindableBase, INavigationAware, IDestructible, IConfirmNavigation, IConfirmNavigationAsync, IPageLifecycleAware, IActiveAware
    {
        protected INavigationService NavigationService { get; private set; }

        protected ILogger Logger { get; private set; }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }

            set
            {
                _isBusy = value;
                SetProperty(ref _isBusy, value);
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    RaisePropertyChanged(nameof(IsBusy));
                });
            }
        }

        private bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value, () => Console.WriteLine($"{GetType().Name}: IsActive: {value}"));
        }

        public event EventHandler IsActiveChanged;

        public ViewModelBase(INavigationService navigationService, ILogger logger)
        {
            NavigationService = navigationService;
            Logger = logger;
        }

        public virtual void Destroy()
        {

        }
        public bool CanNavigate(INavigationParameters parameters)
        {
            return true;
        }

        public void OnAppearing()
        {
        }

        public void OnDisappearing()
        {
        }

        public Task<bool> CanNavigateAsync(INavigationParameters parameters)
        {
            return Task.FromResult(true);
        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {
        }

        public void OnNavigatingTo(INavigationParameters parameters)
        {
        }
    }
}
