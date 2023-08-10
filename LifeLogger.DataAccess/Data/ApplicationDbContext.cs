using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LifeLogger.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LifeLogger.DataAccess.Data
{
    public class ApplicationDbContext:IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
            
        }
        public DbSet<ApplicationUser> ApplicationUsers{get;set;}

        public DbSet<LifeEvent> LifeEvents{get;set;}
    }
}