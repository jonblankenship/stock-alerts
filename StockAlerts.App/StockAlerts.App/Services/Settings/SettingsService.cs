using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace StockAlerts.App.Services.Settings
{
    public class SettingsService : ISettingsService
    {
        #region Setting Constants

        private const string AccessToken = "access_token";
        private readonly string AccessTokenDefault = string.Empty;

        private const string RefreshToken = "refresh_token";
        private readonly string RefreshTokenDefault = string.Empty;
        #endregion
        
        public string AuthAccessToken
        {
            get => Preferences.Get(AccessToken, AccessTokenDefault);
            set => Preferences.Set(AccessToken, value);
        }

        public string AuthRefreshToken
        {
            get => Preferences.Get(RefreshToken, RefreshTokenDefault);
            set => Preferences.Set(RefreshToken, value);
        }
    }
}
