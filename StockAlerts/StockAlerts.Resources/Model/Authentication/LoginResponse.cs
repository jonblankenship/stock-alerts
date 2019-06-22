using System.Collections.Generic;

namespace StockAlerts.Resources.Model.Authentication
{
    public class LoginResponse
    {
        public AccessToken AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public IEnumerable<Error> Errors { get; set; }
    }
}
