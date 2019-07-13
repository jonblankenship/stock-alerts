using StockAlerts.App.ViewModels.AlertDefinitions;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using StockAlerts.Resources.Model;

namespace StockAlerts.App.Services.AlertDefinitions
{
    public interface IAlertDefinitionsService
    {
        Task<ObservableCollection<AlertDefinitionItemViewModel>> GetAlertDefinitionItemViewModelsAsync();

        Task<AlertDefinition> SaveAlertDefinitionAsync(AlertDefinition alertDefinition);
    }
}
