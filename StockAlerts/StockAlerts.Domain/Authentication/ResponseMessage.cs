using System;
using System.Collections.Generic;
using System.Text;

namespace StockAlerts.Domain.Authentication
{
    public abstract class ResponseMessage
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        protected ResponseMessage(bool success = false, string message = null)
        {
            Success = success;
            Message = message;
        }
    }
}
