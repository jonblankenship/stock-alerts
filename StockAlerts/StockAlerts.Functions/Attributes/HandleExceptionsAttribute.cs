using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PostSharp.Aspects;
using StockAlerts.Domain.Exceptions;
using System;
using System.Net;

namespace StockAlerts.Functions.Attributes
{
    [Serializable]
    public class HandleExceptionsAttribute : OnExceptionAspect
    {
        public override void OnException(MethodExecutionArgs args)
        {
            var exception = args.Exception;

            args.FlowBehavior = FlowBehavior.Return;
            
            HttpStatusCode status;
            string message;
            string stackTrace = string.Empty;

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
            
            args.ReturnValue = new ContentResult
            {
                ContentType = "application/json",
                StatusCode = (int)status,
                Content = payload
            };
        }
    }
}
