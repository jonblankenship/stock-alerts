using System.Collections.Generic;
using System.Threading.Tasks;
using StockAlerts.Forms.Models.Permissions;

namespace StockAlerts.Forms.Services.Permissions
{
    public interface IPermissionsService
    {
        Task<PermissionStatus> CheckPermissionStatusAsync(Permission permission);

        Task<Dictionary<Permission, PermissionStatus>> RequestPermissionsAsync(params Permission[] permissions);
    }
}
