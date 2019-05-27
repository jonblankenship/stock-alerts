using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StockAlerts.Domain.Model;
using StockAlerts.Domain.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StockAlerts.Data.Repositories
{
    public class ApiCallsRepository : IApiCallsRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public ApiCallsRepository(
            ApplicationDbContext dbContext,
            IMapper mapper)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task SaveAsync(ApiCall apiCall)
        {
            // Always an Insert, never an update
            var dataObject = _mapper.Map<Data.Model.ApiCall>(apiCall);
            await _dbContext.ApiCalls.AddAsync(dataObject);
            await _dbContext.SaveChangesAsync();

            // Don't worry about mapping back to domain - these are quick log records
        }
    }
}
