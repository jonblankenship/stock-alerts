using StockAlerts.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StockAlerts.Domain.Exceptions;
using StockAlerts.Domain.Repositories;

namespace StockAlerts.Data.Repositories
{
    public class StocksRepository : IStocksRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public StocksRepository(
            ApplicationDbContext dbContext,
            IMapper mapper)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Stock> GetStockAsync(Guid stockId)
        {
            var query = from s in _dbContext.Stocks
                        where s.StockId == stockId
                        select s;

            var dataObject = await query.SingleOrDefaultAsync();
            if (dataObject == null)
                throw new NotFoundException($"Stock with stockId {stockId} not found.");

            return _mapper.Map<Stock>(dataObject);
        }

        public async Task<IEnumerable<Stock>> FindStocksAsync(string symbolStartsWith)
        {
            var query = from s in _dbContext.Stocks
                        where s.Symbol.ToUpper().StartsWith(symbolStartsWith.ToUpper())
                        select s;

            var dataObjects = await query.ToListAsync();

            return _mapper.Map<List<Stock>>(dataObjects);
        }
    }
}
