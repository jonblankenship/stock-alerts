using StockAlerts.App.ViewModels.AlertDefinitions;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace StockAlerts.App.Services.AlertDefinitions
{
    public interface IAlertDefinitionsService
    {
        Task<ObservableCollection<AlertDefinitionItemViewModel>> GetAlertDefinitionItemViewModelsAsync();
    }
}
