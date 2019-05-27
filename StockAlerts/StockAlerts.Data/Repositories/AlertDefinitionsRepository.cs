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
    public class AlertDefinitionsRepository : IAlertDefinitionsRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public AlertDefinitionsRepository(
            ApplicationDbContext dbContext,
            IMapper mapper)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<AlertDefinition>> GetAlertDefinitionsAsync(Guid appUserId)
        {
            var query = from a in _dbContext.AlertDefinitions
                where a.AppUserId == appUserId
                select a;

            var dataObjects = await query.ToListAsync();

            return dataObjects.Select(a => _mapper.Map<AlertDefinition>(a)).ToList();
        }

        public async Task<AlertDefinition> GetAlertDefinitionAsync(Guid alertDefinitionId)
        {
            var query = from a in _dbContext.AlertDefinitions
                    .Include(x => x.AppUser)
                    .ThenInclude(x => x.UserPreferences)
                    .Include(x => x.Stock)
                where a.AlertDefinitionId == alertDefinitionId
                select a;

            var dataObject = await query.SingleOrDefaultAsync();

            if (dataObject == null)
                throw new NotFoundException($"AlertDefinition with id {alertDefinitionId} not found.");

            return _mapper.Map<AlertDefinition>(dataObject);
        }

        public async Task<IEnumerable<AlertDefinition>> GetAlertDefinitionsByStockIdAsync(Guid stockId)
        {
            var query = from a in _dbContext.AlertDefinitions
                        where a.StockId == stockId
                        select a;

            var dataObjects = await query.ToListAsync();

            return dataObjects.Select(a => _mapper.Map<AlertDefinition>(a)).ToList();
        }

        public async Task SaveAsync(AlertDefinition alertDefinition)
        {
            if (alertDefinition.AlertDefinitionId == Guid.Empty)
                await InsertAsync(alertDefinition);
            else
                await UpdateAsync(alertDefinition);
        }

        private async Task InsertAsync(AlertDefinition alertDefinition)
        {
            var dataObject = _mapper.Map<Data.Model.AlertDefinition>(alertDefinition);
            await _dbContext.AlertDefinitions.AddAsync(dataObject);
            await _dbContext.SaveChangesAsync();
            _mapper.Map<AlertDefinition>(dataObject);
        }

        private async Task UpdateAsync(AlertDefinition alertDefinition)
        {
            var dataObject = await (from a in _dbContext.AlertDefinitions
                where a.AlertDefinitionId == alertDefinition.AlertDefinitionId
                select a).SingleAsync();

            _mapper.Map(alertDefinition, dataObject);
            _dbContext.Update(dataObject);
            await _dbContext.SaveChangesAsync();
            _dbContext.Entry(dataObject).State = EntityState.Detached;
            _mapper.Map<AlertDefinition>(dataObject);
        }
    }
}
