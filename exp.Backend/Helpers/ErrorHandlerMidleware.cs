using exp.Models.Helpers;
using Serilog;
using System.Net;
using System.Text.Json;

namespace exp.Backend.Helpers
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                var messageResponse = "Something went wrong";
                
                switch (error)
                {
                    case NotFoundException e:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    case ForbiddenException e:
                        response.StatusCode = (int)HttpStatusCode.Forbidden;
                        break;
                    case ClientException e:
                        response.StatusCode = (int)HttpStatusCode.Conflict;
                        messageResponse = e.Message;
                        break;
                    default:
                        // unhandled error
                        Log.Error(error, error.Message);
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                var result = JsonSerializer.Serialize(new { message = messageResponse });
                await response.WriteAsync(result);
            }
        }
    }
}
