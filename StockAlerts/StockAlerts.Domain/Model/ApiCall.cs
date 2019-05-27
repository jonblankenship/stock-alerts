using StockAlerts.Domain.Repositories;
using System;
using System.Net;
using System.Threading.Tasks;

namespace StockAlerts.Domain.Model
{
    public class ApiCall
    {
        private readonly IApiCallsRepository _apiCallsRepository;

        public ApiCall(IApiCallsRepository apiCallsRepository)
        {
            _apiCallsRepository = apiCallsRepository ?? throw new ArgumentNullException(nameof(apiCallsRepository));
        }

        public Guid ApiCallId { get; set; }

        public string Api { get; set; }

        public string Route { get; set; }

        public HttpStatusCode ResponseCode { get; set; }

        public DateTimeOffset CallTime { get; set; }

        public async Task SaveAsync()
        {
            if (_apiCallsRepository == null)
                throw new ApplicationException($"{nameof(ApiCall)} instantiated without an {nameof(IApiCallsRepository)}.");

            await _apiCallsRepository.SaveAsync(this);
        }
    }
}
