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

        public DbSet<LifeProject> LifeProjects{get;set;}
    }
}