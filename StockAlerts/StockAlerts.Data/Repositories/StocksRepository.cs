using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StockAlerts.Domain.Enums;
using StockAlerts.Domain.Exceptions;
using StockAlerts.Domain.Model;
using StockAlerts.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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

        public async Task<Stock> GetStockAsync(string symbol)
        {
            var query = from s in _dbContext.Stocks
                where s.Symbol == symbol
                select s;

            var dataObject = await query.SingleOrDefaultAsync();
            if (dataObject != null)
                return _mapper.Map<Stock>(dataObject);

            return null;
        }

        public async Task<IEnumerable<Stock>> FindStocksAsync(
            string symbolStartsWith,
            CancellationToken cancellationToken)
        {
            var searchString = symbolStartsWith.ToUpper();
            var query = from s in _dbContext.Stocks
                        where s.Symbol.ToUpper().StartsWith(searchString) ||
                              s.Name.ToUpper().StartsWith(searchString)
                        select s;

            var dataObjects = await query.ToListAsync(cancellationToken);

            return _mapper.Map<List<Stock>>(dataObjects);
        }

        public async Task<IEnumerable<Stock>> GetSubscribedStocksAsync()
        {
            var query = (from a in _dbContext.AlertDefinitions
                         where a.Status == AlertDefinitionStatuses.Enabled
                         select a.Stock).Distinct();

            var dataObjects = await query.ToListAsync();
            return _mapper.Map<List<Stock>>(dataObjects);
        }

        public async Task SaveAsync(Stock stock)
        {
            if (stock.StockId == Guid.Empty)
                await InsertAsync(stock);
            else
                await UpdateAsync(stock);
        }

        private async Task InsertAsync(Stock stock)
        {
            var dataObject = _mapper.Map<Data.Model.Stock>(stock);
            await _dbContext.Stocks.AddAsync(dataObject);
            await _dbContext.SaveChangesAsync();
            _mapper.Map<Stock>(dataObject);
        }

        private async Task UpdateAsync(Stock stock)
        {
            var dataObject = await (from s in _dbContext.Stocks
                                    where s.StockId == stock.StockId
                                    select s).SingleAsync();

            _mapper.Map(stock, dataObject);
            _dbContext.Update(dataObject);
            await _dbContext.SaveChangesAsync();
            _dbContext.Entry(dataObject).State = EntityState.Detached;
            _mapper.Map<Stock>(dataObject);
        }
    }
}
