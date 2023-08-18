
using System.Net;
using LifeLogger.API.Middleware.Exceptions;
using LifeLogger.Models.DTO;
using Newtonsoft.Json;

namespace LifeLogger.API.Middleware.Configurations
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private APIResponse response;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next)
        {

            _next = next;
            response = new();
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {

                await HandleExceptionAsync(httpContext, ex);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var exceptionType = ex.GetType();
            if(exceptionType == typeof(NotFoundException))
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.IsSuccess = false;
                response.ErrorMessage = new List<string> { ex.Message };
            }
            else if(exceptionType == typeof(BadRequestException))
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.IsSuccess = false;
                response.ErrorMessage = new List<string> { ex.Message };
            }
            else
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.IsSuccess = false;
                response.ErrorMessage = new List<string> { ex.Message };
            }
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var _response = JsonConvert.SerializeObject(response);
            await context.Response.WriteAsync(_response);
        }
    }
}