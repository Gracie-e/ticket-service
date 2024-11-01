using System.Net;
using System.Text.Json;

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
            logger.LogError(ex, "An unexpected error occured.");
            
            var response = context.Response;
            response.ContentType = "application/json";

            var errorResponse = new
            {
                Message = "An error occured while processing your request",
                StatusCode = (int)HttpStatusCode.InternalServerError,
            };

            switch (ex)
            {
                case ArgumentNullException:
                case InvalidOperationException:
                case ArgumentException:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse = new { ex.Message, StatusCode = (int)HttpStatusCode.BadRequest };
                    break;
                
                case KeyNotFoundException:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    errorResponse = new { ex.Message, StatusCode = (int)HttpStatusCode.NotFound };
                    break;
                
                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }
            
            var result = JsonSerializer.Serialize(errorResponse);
            await response.WriteAsync(result);
        }
    }
}