using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LifeLogger.API.Middleware.Configurations;

namespace LifeLogger.API.Extensions
{
    public static class ExceptionHandlerExtension
    {
        public static IApplicationBuilder AddGlobalExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
            return app;
        }
    }
}