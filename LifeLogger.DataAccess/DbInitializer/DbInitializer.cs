using Microsoft.EntityFrameworkCore;
using LifeLogger.DataAccess.Data;
using Microsoft.AspNetCore.Identity;
using LifeLogger.Utility;
using LifeLogger.Models;

namespace LifeLogger.DataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;

        public DbInitializer(UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager,ApplicationDbContext db)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
        }

        public void Initialize( )
        {
            //apply if any pending migration

             try {
                if (_db.Database.GetPendingMigrationsAsync().GetAwaiter().GetResult().Count() > 0) {
                    _db.Database.MigrateAsync().GetAwaiter().GetResult();
                }
            }
            catch(Exception ex) {
                Console.WriteLine(ex);
            }

            //create roles if they are not created

            if( !_roleManager.RoleExistsAsync(SD.Role_Admin).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Role_User)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Collaborator)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();



                // if role doesnot exist then user also doesnot exist

                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "testadmin@test.de",
                    Email = "testadmin@test.de",
                    Name = "TestAdmin",
                    PhoneNumber = "1112223333",
                    StreetAddress = "test 123 Ave",
                    State = "IL",
                    PostalCode = "23422",
                    City = "Chicago",
                    ProfilePicturePath = "Nothing for now",
                     DateJoined = DateTime.Now
                }, "Admin123*").GetAwaiter().GetResult();

                ApplicationUser user = _db.ApplicationUsers.FirstOrDefault(u => u.Email == "testadmin@test.de");
                _userManager.AddToRoleAsync(user,SD.Role_Admin).GetAwaiter().GetResult();

                // After user is created , create event related to the user

                var events = new List<LifeProject>
                {
                    new LifeProject
                    {
                        UserID = user.Id,
                        Title = "Graduation",
                        Description = "To be a master graduate",
                        Location ="Deutschland",
                        StartTime = DateTime.Now,
                        IsPublic = false,
                        CreatedAt = DateTime.Now
                     },
                    new LifeProject
                    {
                        UserID = user.Id,
                        Title = "Job",
                        Description = "To have a new good job",
                        Location ="Deutschland",
                        StartTime = DateTime.Now,
                        IsPublic = false,
                        CreatedAt = DateTime.Now

                    },
                    new LifeProject
                    {
                        UserID = user.Id,
                        Title = "Fitness",
                        Description = "To be able to exercise",
                        Location ="Deutschland",
                        StartTime = DateTime.Now,
                        IsPublic = false,
                        CreatedAt = DateTime.Now
                    }
                };

                _db.LifeProjects.AddRangeAsync(events).GetAwaiter().GetResult();
                _db.SaveChangesAsync().GetAwaiter().GetResult();
            }

            return;

        }
    }
}