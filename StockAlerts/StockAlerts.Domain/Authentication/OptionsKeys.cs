using System;
using System.Collections.Generic;
using System.Text;

namespace StockAlerts.Domain.Authentication
{
    public static class OptionsKeys
    {
        public static class Jwt
        {
            public const string Key = "Jwt:Key";

            public const string Issuer = "Jwt:Issuer";

            public const string Audience = "Jwt:Audience";

            public const string ExpireTime = "Jwt:ExpireTime";
        }
    }
}
