using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StockAlerts.Domain.Exceptions;
using StockAlerts.Domain.Model;
using StockAlerts.Domain.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StockAlerts.Data.Repositories
{
    public class UserPreferencesRepository : IUserPreferencesRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public UserPreferencesRepository(
            ApplicationDbContext dbContext,
            IMapper mapper)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<UserPreferences> GetUserPreferencesAsync(Guid appUserId)
        {
            var query = from a in _dbContext.AppUsers
                        where a.AppUserId == appUserId
                        select a.UserPreferences;

            var dataObject = await query.SingleOrDefaultAsync();
            if (dataObject == null)
                throw new NotFoundException($"AppUser with id {appUserId} not found.");

            return _mapper.Map<UserPreferences>(dataObject);
        }

        public async Task SaveAsync(UserPreferences userPreferences)
        {
            if (userPreferences.UserPreferencesId == Guid.Empty)
                await InsertAsync(userPreferences);
            else
                await UpdateAsync(userPreferences);
        }

        private async Task InsertAsync(UserPreferences userPreferences)
        {
            var dataObject = _mapper.Map<Data.Model.UserPreferences>(userPreferences);
            await _dbContext.AddAsync(dataObject);
            await _dbContext.SaveChangesAsync();

            _mapper.Map(dataObject, userPreferences);
        }

        private async Task UpdateAsync(UserPreferences userPreferences)
        {
            var dataObject = await (from a in _dbContext.AppUsers
                                    where a.AppUserId == userPreferences.AppUserId
                                    select a.UserPreferences).SingleAsync();
            
            _mapper.Map(userPreferences, dataObject);
            _dbContext.Update(dataObject);
            await _dbContext.SaveChangesAsync();

            _mapper.Map(dataObject, userPreferences);
        }
    }
}
