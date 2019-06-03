using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.Amqp.Framing;

namespace StockAlerts.Domain.Authentication
{

    public class LoginResponse : ResponseMessage
    {
        public class LoginErrorCodes
        {
            public const string AwaitingAccess = "AwaitingAccess";
        }

        public AccessToken AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public IEnumerable<Error> Errors { get; set; }

        public LoginResponse() { }

        public LoginResponse(IEnumerable<Error> errors, bool success = false, string message = null) : base(success, message)
        {
            Errors = errors;
        }

        public LoginResponse(AccessToken accessToken, string refreshToken, bool success = false, string message = null) : base(success, message)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }
}
