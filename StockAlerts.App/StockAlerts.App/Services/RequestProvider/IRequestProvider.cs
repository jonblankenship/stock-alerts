using System.Threading;
using System.Threading.Tasks;

namespace StockAlerts.App.Services.RequestProvider
{

    public interface IRequestProvider
    {


        Task<TResult> GetAsync<TResult>(string uri);

        Task<TResult> GetAsync<TResult>(string uri, string token);

        Task<TResult> GetAsync<TResult>(string uri, string token, CancellationToken cancellationToken);

        Task<TResult> PostAsync<TData, TResult>(string uri, TData data, string token = "", string header = "");

        Task<TResult> PostAsync<TResult>(string uri, TResult data, string token = "", string header = "");

        Task<TResult> PutAsync<TResult>(string uri, TResult data, string token = "", string header = "");

        Task DeleteAsync(string uri, string token = "");
    }
}
