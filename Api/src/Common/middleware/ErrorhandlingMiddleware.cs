using Api.src.Common.exceptions;
using System.Net;
using System.Text.Json;

namespace Api.src.Common.middleware
{
    public class ErrorhandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorhandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }

            catch (Exception ex)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                var body = new { ex.Message };
                switch (ex)
                {
                    case NotFoundException e:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    case DuplicateException e:
                        response.StatusCode = (int)HttpStatusCode.Conflict;
                        break;
                    case ValidationException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }
                var result = JsonSerializer.Serialize(body);
                await response.WriteAsync(result);

            }
        }


    }
}
