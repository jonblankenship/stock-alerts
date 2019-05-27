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

        public async Task<AppUser> GetAppUserAsync(Guid appUserId)
        {
            var query = from a in _dbContext.AppUsers.Include(x => x.UserPreferences)
                where a.AppUserId == appUserId
                        select a;

            var dataObject = await query.SingleOrDefaultAsync();

            if (dataObject == null)
                throw new NotFoundException($"AppUser with id {appUserId} not found.");

            return _mapper.Map<AppUser>(dataObject);
        }
    }
}
