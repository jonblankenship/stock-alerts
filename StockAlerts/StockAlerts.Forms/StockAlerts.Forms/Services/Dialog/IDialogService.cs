using System.Threading.Tasks;

namespace StockAlerts.Forms.Services.Dialog
{
    public interface IDialogService
    {
        Task ShowAlertAsync(string message, string title, string buttonLabel);
    }
}
