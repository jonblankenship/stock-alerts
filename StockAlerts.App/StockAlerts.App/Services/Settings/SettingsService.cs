using System;
using System.Threading.Tasks;
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

        #region Settings Properties

        public string AuthAccessToken
        {
            get => GetValueOrDefault(AccessToken, AccessTokenDefault);
            set => AddOrUpdateValue(AccessToken, value);
        }

        public string AuthRefreshToken
        {
            get => GetValueOrDefault(RefreshToken, RefreshTokenDefault);
            set => AddOrUpdateValue(RefreshToken, value);
        }

        #endregion

        #region Public Methods

        public Task AddOrUpdateValue(string key, bool value) => AddOrUpdateValueInternal(key, value);
        public Task AddOrUpdateValue(string key, string value) => AddOrUpdateValueInternal(key, value);
        public bool GetValueOrDefault(string key, bool defaultValue) => GetValueOrDefaultInternal(key, defaultValue);
        public string GetValueOrDefault(string key, string defaultValue) => GetValueOrDefaultInternal(key, defaultValue);

        #endregion

        #region Internal Implementation

        async Task AddOrUpdateValueInternal<T>(string key, T value)
        {
            if (value == null)
            {
                await Remove(key);
            }

            Application.Current.Properties[key] = value;
            try
            {
                await Application.Current.SavePropertiesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to save: " + key, " Message: " + ex.Message);
            }
        }

        T GetValueOrDefaultInternal<T>(string key, T defaultValue = default(T))
        {
            object value = null;
            if (Application.Current.Properties.ContainsKey(key))
            {
                value = Application.Current.Properties[key];
            }
            return null != value ? (T)value : defaultValue;
        }

        async Task Remove(string key)
        {
            try
            {
                if (Application.Current.Properties[key] != null)
                {
                    Application.Current.Properties.Remove(key);
                    await Application.Current.SavePropertiesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to remove: " + key, " Message: " + ex.Message);
            }
        }

        #endregion
    }
}
