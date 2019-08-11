using System;
using System.Net;
using System.Threading.Tasks;
using Api.Auth.Models.Exceptions;
using Api.Auth.Models.Responses;
using Microsoft.AspNetCore.Http;
using NLog.Fluent;

namespace Api.Auth.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                Log.Error($"Exception: {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            switch (exception)
            {
                case BadRequestException _:
                    context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                    break;
            }

            return context.Response.WriteAsync(new ErrorResponses.ErrorResponse
            {
                StatusCode = context.Response.StatusCode,
                Message = exception.Message
            }.ToString());
        }

    }
}