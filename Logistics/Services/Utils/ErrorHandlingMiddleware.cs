using Logistics.Data.Common;
using Logistics.Data.Common.CommonDTOs.Responses;
using System.Text.Json;

namespace Logistics.Services.Utils
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (CustomException exception)
            {
                await HandleExceptionAsync(context, exception);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, CustomException exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = exception.status;

            var result = JsonSerializer.Serialize(new ErrorResponse(exception.status, exception.message));
            return context.Response.WriteAsync(result);
        }
    }
}
