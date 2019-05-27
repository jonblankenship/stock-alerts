using StockAlerts.Domain.Model;
using System;
using System.Threading.Tasks;

namespace StockAlerts.Domain.Repositories
{
    public interface IAppUsersRepository
    {
        /// <summary>
        /// Retrieves the <see cref="AppUser"/> for the given <see cref="appUserId"/>
        /// </summary>
        /// <param name="appUserId"></param>
        /// <returns></returns>
        Task<AppUser> GetAppUserAsync(Guid appUserId);
    }
}
