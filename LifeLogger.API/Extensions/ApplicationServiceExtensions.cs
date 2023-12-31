using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LifeLogger.API.Middleware.Exceptions;
using LifeLogger.DataAccess.Data;
using LifeLogger.DataAccess.DbInitializer;
using LifeLogger.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LifeLogger.API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection service, IConfiguration config)
        {
            service.AddDbContext<ApplicationDbContext>(option =>
            {
                option.UseSqlServer(config.GetConnectionString("DefaultSQLConnection"));
            });
            service.Configure<ApiBehaviorOptions>(options =>
            {
                // options.SuppressModelStateInvalidFilter = true;
               options.InvalidModelStateResponseFactory = context =>
                {
                   return  CustomBadRequestException.ConstructErrorMessages(context);        
                };
            });
            service.AddAutoMapper(typeof(MappingConfig));
            service.AddScoped<IDbInitializer, DbInitializer>();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            service.AddEndpointsApiExplorer();
            service.AddSwaggerGen();
           return service;
        }


        
    }
}