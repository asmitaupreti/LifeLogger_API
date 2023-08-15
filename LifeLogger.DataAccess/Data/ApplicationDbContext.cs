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

        public DbSet<LifeMilestone> LifeMilestones{get;set;}

        public DbSet<LifeIncident> LifeIncidents{get;set;}

        public DbSet<Media> Medias{get;set;}

        public DbSet<LifeReport> LifeReports{get;set;}

        public DbSet<Tag> Tags{get;set;}

        public DbSet<MilestoneTagMapping> MilestoneTagMappings{get;set;}

        public DbSet<IncidentMediaMapping> IncidentMediaMappings{get;set;}

        public DbSet<MilestoneReportMapping> MilestoneReportMappings{get;set;}


        protected override void OnModelCreating(ModelBuilder modelBuilder)    
        {                     
            base.OnModelCreating(modelBuilder);
                                        
            modelBuilder.Entity<MilestoneTagMapping>()             
                .HasKey(x => new {x.MilestoneID, x.TagID});

            modelBuilder.Entity<IncidentMediaMapping>()             
                .HasKey(x => new {x.IncidentId, x.MediaId});

             modelBuilder.Entity<MilestoneReportMapping>()             
                .HasKey(x => new {x.MilestoneId, x.ReportId});
        }   
    }
}