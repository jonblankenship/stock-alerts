using StockAlerts.Resources.Model;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using StockAlerts.App.ViewModels;
using StockAlerts.App.ViewModels.AlertDefinitions;

namespace StockAlerts.App.Services.AlertDefinitions
{
    public interface IAlertDefinitionsService
    {
        Task<ObservableCollection<AlertDefinitionItemViewModel>> GetAlertDefinitionItemViewModelsAsync();
    }
}
