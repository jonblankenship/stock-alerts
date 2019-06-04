using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using StockAlerts.Domain.Repositories;

namespace StockAlerts.Domain.Model
{
    public class AppUser
    {
        private readonly IAppUsersRepository _appUsersRepository;
        private readonly List<RefreshToken> _deletedRefreshTokens = new List<RefreshToken>();

        public AppUser() { }

        public AppUser(IAppUsersRepository appUsersRepository)
        {
            _appUsersRepository = appUsersRepository ?? throw new ArgumentNullException(nameof(appUsersRepository));
        }

        public Guid AppUserId { get; set; }

        public Guid UserId { get; set; }

        public string UserName { get; set; }

        public bool HasBeenGrantedAccess { get; set; }

        private List<RefreshToken> _refreshTokens = new List<RefreshToken>();
        public List<RefreshToken> RefreshTokens
        {
            get => _refreshTokens;
            set => _refreshTokens = value;
        }

        public UserPreferences UserPreferences { get; set; }

        public Task SaveAsync(CancellationToken cancellationToken) => _appUsersRepository.SaveAsync(this, cancellationToken);


        public bool HasValidRefreshToken(string refreshToken)
        {
            return _refreshTokens.Any(rt => rt.Token == refreshToken && rt.Active);
        }

        public void AddRefreshToken(string token, Guid userId, string remoteIpAddress, double daysToExpire = 5)
        {
            _refreshTokens.Add(new RefreshToken(token, DateTime.UtcNow.AddDays(daysToExpire), userId, remoteIpAddress));
        }

        public void RemoveRefreshToken(string refreshToken)
        {
            var refreshTokenToDelete = _refreshTokens.First(t => t.Token == refreshToken);
            _deletedRefreshTokens.Add(refreshTokenToDelete);
            _refreshTokens.Remove(refreshTokenToDelete);
        }
    }
}
