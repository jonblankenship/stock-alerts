using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using StockAlerts.Resources.Model;

namespace StockAlerts.App.Services.RequestProvider
{
    public class HttpRequestExceptionEx : HttpRequestException
    {
        public System.Net.HttpStatusCode HttpCode { get; }

        public HttpError Error { get; private set; }

        public HttpRequestExceptionEx(System.Net.HttpStatusCode code) : this(code, null, null)
        {
        }

        public HttpRequestExceptionEx(System.Net.HttpStatusCode code, string message) : this(code, message, null)
        {
            // There's probably a better way to do this
            if (message.ToLower().Contains("\"error\":") && message.ToLower().Contains("\"stacktrace\":"))
            {
                Error = JsonConvert.DeserializeObject<HttpError>(message);
            }
        }

        public HttpRequestExceptionEx(System.Net.HttpStatusCode code, string message, Exception inner) : base(message,
            inner)
        {
            HttpCode = code;
        }

    }
}
