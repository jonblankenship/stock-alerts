using Nito.Mvvm;
using Prism.Navigation;
using StockAlerts.App.Services.Logging;
using StockAlerts.App.Services.Stocks;
using StockAlerts.App.ViewModels.Base;
using StockAlerts.Resources.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using StockAlerts.App.Constants;

namespace StockAlerts.App.ViewModels.AlertDefinitions
{
    public class StockSearchPageViewModel : ViewModelBase
    {
        private readonly IStocksService _stocksService;
        private CancellationTokenSource _searchCancellationTokenSource;

        public StockSearchPageViewModel(
            IStocksService stocksService,
            INavigationService navigationService, 
            ILogger logger) : base(navigationService, logger)
        {
            _stocksService = stocksService ?? throw new ArgumentNullException(nameof(stocksService));
        }

        private string _searchString;

        public string SearchString
        {
            get => _searchString;
            set
            {
                _searchString = value;
                
                var newSearchCancellationTokenSource = new CancellationTokenSource();
                if (_searchCancellationTokenSource != null)
                    _searchCancellationTokenSource.Cancel();
                _searchCancellationTokenSource = newSearchCancellationTokenSource;

                Stocks = NotifyTask.Create(SearchStocksAsync(_searchCancellationTokenSource));
                RaisePropertyChanged(nameof(SearchString));
            }
        }

        private NotifyTask<List<Stock>> _stocks;
        public NotifyTask<List<Stock>> Stocks
        {
            get => _stocks;
            set
            {
                _stocks = value;
                RaisePropertyChanged(nameof(Stocks));
            }
        }

        private Stock _stock;
        public Stock SelectedStock
        {
            get => _stock;
            set
            {
                _stock = value;
                var navigationParams = new NavigationParameters();
                navigationParams.Add(NavigationParameterKeys.SelectedStock, _stock);
                NavigationService.GoBackAsync(navigationParams);
            }
        }

        private async Task<List<Stock>> SearchStocksAsync(CancellationTokenSource searchCancellationTokenSource)
        {
            if (SearchString.Length >= 1)
            {
                await Task.Delay(1000);
                try
                {
                    if (!searchCancellationTokenSource.IsCancellationRequested)
                    {
                        var stocks = await _stocksService.FindStocksAsync(SearchString, searchCancellationTokenSource.Token);
                        return stocks.ToList();
                    }
                }
                finally
                {
                    searchCancellationTokenSource.Dispose();
                    _searchCancellationTokenSource = null;
                }
            }

            return new List<Stock>();
        }
    }
}
