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
                Stocks = NotifyTask.Create(SearchStocksAsync());
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

        private async Task<List<Stock>> SearchStocksAsync()
        {
            if (_searchCancellationTokenSource != null)
                _searchCancellationTokenSource.Cancel();

            if (SearchString.Length >= 1)
            {
                using (_searchCancellationTokenSource = new CancellationTokenSource())
                {
                    var stocks = await _stocksService.FindStocksAsync(SearchString, CancellationToken.None);
                    return stocks.ToList();
                }
            }

            return new List<Stock>();
        }
    }
}
