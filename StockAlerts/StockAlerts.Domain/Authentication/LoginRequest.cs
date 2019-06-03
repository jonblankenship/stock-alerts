using System;
using System.Collections.Generic;
using System.Text;

namespace StockAlerts.Domain.Authentication
{
    public class LoginRequest
    {
        public LoginRequest(string userName, string password, string remoteIpAddress)
        {
            Username = userName;
            Password = password;
            RemoteIpAddress = remoteIpAddress;
        }

        public LoginRequest(RegisterRequest registerRequest)
        {
            Username = registerRequest.Username;
            Password = registerRequest.Password;
            RemoteIpAddress = registerRequest.RemoteIpAddress;
        }

        public string Username { get; set; }

        public string Password { get; set; }

        public string RemoteIpAddress { get; set; }
    }
}
