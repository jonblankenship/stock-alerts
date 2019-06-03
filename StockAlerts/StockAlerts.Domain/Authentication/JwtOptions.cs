using System;
using System.Collections.Generic;
using System.Text;

namespace StockAlerts.Domain.Authentication
{
    public class JwtOptions
    {
        public string Key { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }

        public int ExpireTime { get; set; }
    }
}
