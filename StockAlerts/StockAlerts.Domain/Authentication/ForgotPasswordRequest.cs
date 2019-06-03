using System;
using System.Collections.Generic;
using System.Text;
using StockAlerts.Domain.Exceptions;

namespace StockAlerts.Domain.Authentication
{

    public class ForgotPasswordRequest
    {
        public ForgotPasswordRequest() { }

        public ForgotPasswordRequest(string email, string remoteIpAddress)
        {
            Email = email;
            RemoteIpAddress = remoteIpAddress;
        }

        public string Email { get; set; }

        public string RemoteIpAddress { get; set; }


        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Email))
            {
                throw new BadRequestException($"{nameof(Email)} is required.");
            }
        }
    }
}
