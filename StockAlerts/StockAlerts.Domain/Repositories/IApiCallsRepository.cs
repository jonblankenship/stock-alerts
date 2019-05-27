using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StockAlerts.Domain.Model;

namespace StockAlerts.Domain.Repositories
{
    public interface IApiCallsRepository
    {
        /// <summary>
        /// Persists <see cref="apidCall"/> to the database
        /// </summary>
        /// <param name="stock"></param>
        /// <returns></returns>
        Task SaveAsync(ApiCall apiCall);
    }
}