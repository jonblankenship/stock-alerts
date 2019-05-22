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
                        where a.AlertDefinitionId == alertDefinitionId
                        select a;

            var dataObject = await query.SingleOrDefaultAsync();

            if (dataObject == null)
                throw new NotFoundException($"AlertDefinition with id {alertDefinitionId} not found.");

            return _mapper.Map<AlertDefinition>(dataObject);
        }
    }
}
