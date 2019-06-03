using StockAlerts.Domain.Model;
using System;
using System.Threading;
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

        /// <summary>
        /// Retrieves the <see cref="AppUser"/> with the given <see cref="userId"/>
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<AppUser> GetAppUserByUserIdAsync(Guid userId, CancellationToken cancellationToken);

        /// <summary>
        /// Saves the <see cref="AppUser"/> to the database
        /// </summary>
        /// <param name="appUser"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task SaveAsync(AppUser appUser, CancellationToken cancellationToken);
    }
}
