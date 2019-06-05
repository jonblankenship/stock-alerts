using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StockAlerts.Domain.Exceptions;
using StockAlerts.Domain.Model;
using StockAlerts.Domain.Repositories;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StockAlerts.Data.Repositories
{
    public class AppUsersRepository : IAppUsersRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public AppUsersRepository(
            ApplicationDbContext dbContext,
            IMapper mapper)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<AppUser> GetAppUserAsync(Guid appUserId, CancellationToken cancellationToken)
        {
            var query = from a in _dbContext.AppUsers
                            .Include(x => x.UserPreferences)
                            .Include(x => x.RefreshTokens)
                        where a.AppUserId == appUserId
                        select a;

            var dataObject = await query.SingleOrDefaultAsync(cancellationToken);

            if (dataObject == null)
                throw new NotFoundException($"AppUser with id {appUserId} not found.");

            return _mapper.Map<AppUser>(dataObject);
        }

        public async Task<AppUser> GetAppUserByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            var query = from a in _dbContext.AppUsers.Include(x => x.UserPreferences)
                        where a.UserId == userId
                        select a;

            var dataObject = await query.SingleOrDefaultAsync(cancellationToken);

            if (dataObject == null)
                throw new NotFoundException($"AppUser with UserId {userId} not found.");

            return _mapper.Map<AppUser>(dataObject);
        }

        public async Task SaveAsync(AppUser appUser, CancellationToken cancellationToken)
        {
            if (appUser.AppUserId == Guid.Empty)
                await InsertAsync(appUser, cancellationToken);
            else
                await UpdateAsync(appUser, cancellationToken);
        }

        private async Task InsertAsync(AppUser appUser, CancellationToken cancellationToken)
        {
            var dataObject = _mapper.Map<Data.Model.AppUser>(appUser);
            await _dbContext.AppUsers.AddAsync(dataObject, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            _mapper.Map(dataObject, appUser);
        }

        private async Task UpdateAsync(AppUser appUser, CancellationToken cancellationToken)
        {
            var dataObject = _mapper.Map<Data.Model.AppUser>(appUser);
            _dbContext.Update(dataObject);
            await _dbContext.SaveChangesAsync(cancellationToken);

            _mapper.Map(dataObject, appUser);

        }
    }
}
