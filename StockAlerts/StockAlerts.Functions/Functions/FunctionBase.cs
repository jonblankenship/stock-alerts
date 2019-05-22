using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StockAlerts.Domain.Exceptions;
using System;
using System.Net;
using System.Threading.Tasks;

namespace StockAlerts.Functions
{
    public abstract class FunctionBase
    {
        public static IActionResult HandleException(
            Exception exception,
            HttpContext context)
        {
            HttpStatusCode status;
            string message;
            string stackTrace = String.Empty;

            var exceptionType = exception.GetType();
            if (exceptionType == typeof(BadRequestException))
            {
                message = exception.Message;
                status = HttpStatusCode.BadRequest;
            }
            else if (exceptionType == typeof(NotFoundException))
            {
                message = exception.Message;
                status = HttpStatusCode.NotFound;
            }
            else if (exceptionType == typeof(UnauthorizedAccessException))
            {
                message = exception.Message;
                status = HttpStatusCode.Unauthorized;
            }
            else
            {
                status = HttpStatusCode.InternalServerError;
                message = exception.Message;
                stackTrace = exception.StackTrace;
            }

            var payload = JsonConvert.SerializeObject(new { error = message, stackTrace });

            var result = new ContentResult
            {
                ContentType = "application/json",
                StatusCode = (int)status,
                Content = payload
            };
            return result;
        }
    }
}
