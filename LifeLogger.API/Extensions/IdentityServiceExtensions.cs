using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LifeLogger.DataAccess.Data;
using LifeLogger.Models;
using Microsoft.AspNetCore.Identity;

namespace LifeLogger.API.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServiceExtensions(this IServiceCollection service, IConfiguration config )
        {
            service.AddIdentity<ApplicationUser,IdentityRole>(opt =>
            {
                opt.Password.RequireDigit = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireLowercase = false;
                opt.User.RequireUniqueEmail = false;

            })
            .AddEntityFrameworkStores<ApplicationDbContext>();

            return service;
        }
    }
}