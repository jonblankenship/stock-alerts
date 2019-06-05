using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StockAlerts.Domain.Exceptions;
using StockAlerts.Domain.Extensions;
using StockAlerts.Domain.Model;
using StockAlerts.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using StockAlerts.Data.Extensions;

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
            var query = from a in _dbContext.AlertDefinitions.IncludeAllRelatedEntities()
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

        public async Task DeleteAsync(AlertDefinition alertDefinition)
        {
            var dataObject = _mapper.Map<Data.Model.AlertDefinition>(alertDefinition);
            var childCriteria = dataObject.RootCriteria.Traverse(x => x.ChildrenCriteria);
            _dbContext.RemoveRange(childCriteria);
            _dbContext.AlertDefinitions.Remove(dataObject);
            await _dbContext.SaveChangesAsync();
        }

        private async Task InsertAsync(AlertDefinition alertDefinition)
        {
            var dataObject = _mapper.Map<Data.Model.AlertDefinition>(alertDefinition);
            await _dbContext.AlertDefinitions.AddAsync(dataObject);
            await _dbContext.SaveChangesAsync();

            _mapper.Map(dataObject, alertDefinition);
        }

        private async Task UpdateAsync(AlertDefinition alertDefinition)
        {
            var dataObject = await (from a in _dbContext.AlertDefinitions.IncludeAllAlertCriteria()
                                    where a.AlertDefinitionId == alertDefinition.AlertDefinitionId
                                    select a).SingleAsync();

            var criteria = dataObject.RootCriteria.Traverse(c => c.ChildrenCriteria);

            var alertCriteriaToDelete = from ac in criteria
                                        where !alertDefinition.ContainsAlertCriteriaId(ac.AlertCriteriaId)
                                        select ac;

            foreach (var ac in alertCriteriaToDelete)
            {
                if (_dbContext.Entry(ac) != null)
                    _dbContext.Entry(ac).State = EntityState.Deleted;
                else
                    _dbContext.Remove(ac);
            }

            _mapper.Map(alertDefinition, dataObject);
            _dbContext.Update(dataObject);
            await _dbContext.SaveChangesAsync();

            _mapper.Map(dataObject, alertDefinition);
        }
    }
}
