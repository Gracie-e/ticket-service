using System.Net;
using System.Text.Json;
using Ticket_Service.Common.Api;

namespace Ticket_Service.Middleware;

public class GlobalExceptionHandlingMiddleware(ILogger<GlobalExceptionHandlingMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred.");
            
            var response = context.Response;
            response.ContentType = "application/json";

            var statusCode = ex switch
            {
                ArgumentNullException or InvalidOperationException or ArgumentException 
                    => (int)HttpStatusCode.BadRequest,
                KeyNotFoundException 
                    => (int)HttpStatusCode.NotFound,
                _ => (int)HttpStatusCode.InternalServerError
            };

            response.StatusCode = statusCode;

            var apiResponse = ApiResponse<object>.Failure(
                message: statusCode == (int)HttpStatusCode.InternalServerError
                    ? "An error occurred while processing your request"
                    : ex.Message,
                statusCode: statusCode
            );
            
            var result = JsonSerializer.Serialize(apiResponse);
            await response.WriteAsync(result);
        }
    }
}