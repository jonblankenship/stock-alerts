using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace StockAlerts.Domain.Authentication
{
    public sealed class AccessToken
    {
        public string Token { get; set; }

        public int ExpiresIn { get; set; }

        public AccessToken(string token, int expiresIn)
        {
            Token = token;
            ExpiresIn = expiresIn;
        }

        public string Header => Token.Split('.').First();

        public string Payload => Token.Split('.').Skip(1).First();

        public string Signature => Token.Split('.').Last();

        public Dictionary<string, string> GetClaims()
        {
            var payload = DecodeBase64String(Payload);

            return JsonConvert.DeserializeObject<Dictionary<string, string>>(payload);
        }

        private string DecodeBase64String(string base64String)
        {
            var lengthMod4 = base64String.Length % 4;
            if (lengthMod4 != 0)
            {
                base64String = base64String.PadRight(base64String.Length + (4 - lengthMod4), '=');
            }

            var base64EncodedBytes = Convert.FromBase64String(base64String);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
