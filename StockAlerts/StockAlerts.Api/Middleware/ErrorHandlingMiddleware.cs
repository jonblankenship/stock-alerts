using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using StockAlerts.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace StockAlerts.Api.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
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

            var result = JsonConvert.SerializeObject(new { error = message, stackTrace });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;
            return context.Response.WriteAsync(result);
        }
    }
}
