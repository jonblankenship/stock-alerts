using System.Collections.Generic;
using System.Threading.Tasks;
using StockAlerts.App.Models.Permissions;

namespace StockAlerts.App.Services.Permissions
{
    public interface IPermissionsService
    {
        Task<PermissionStatus> CheckPermissionStatusAsync(Permission permission);

        Task<Dictionary<Permission, PermissionStatus>> RequestPermissionsAsync(params Permission[] permissions);
    }
}
