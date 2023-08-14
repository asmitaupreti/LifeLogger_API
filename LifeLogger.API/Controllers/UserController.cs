using System;
using LifeLogger.DataAccess.Data;
using LifeLogger.Models;
using LifeLogger.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LifeLogger.API.Controllers
{
    [Route("api/UserAuth")]
    [ApiController]
    public class UserController: Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;
        public UserController(ApplicationDbContext db,UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

         private string FindRole(string userRole)
         {
            switch (userRole)
            {
                case SD.Role_Admin:
                    return SD.Role_Admin;

                case SD.Role_User:
                    return SD.Role_User;

                case SD.Role_Collaborator:
                    return SD.Role_Collaborator;
                    
                default: return SD.Role_Admin;
            }
         }

        [HttpGet]
        public async Task<IActionResult> GetAllUser()
        {
            try
            {
                IEnumerable<ApplicationUser> applicationUserList =  await _db.ApplicationUsers.ToListAsync();
                return Ok(applicationUserList);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception GetAllUser:{ex}");
                return BadRequest();
                
            }
           
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            try
            {
                ApplicationUser user = await _db.ApplicationUsers.Where( u=> u.Id ==id).FirstOrDefaultAsync();
                return Ok(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception GetUser:{ex}");
                return BadRequest();
                
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody]ApplicationUser applicationUser)
        {
            try
            {
                var user = await _db.ApplicationUsers.FirstOrDefaultAsync(u => u.UserName == applicationUser.UserName);
                if(user!= null)
                    return BadRequest();
                else
                {
                    var result = await _userManager.CreateAsync(applicationUser, "Test123*");
                    if(!_roleManager.RoleExistsAsync(SD.Role_Admin).GetAwaiter().GetResult())
                    {
                        await _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin));
                        await _roleManager.CreateAsync(new IdentityRole(SD.Role_User));
                        await _roleManager.CreateAsync(new IdentityRole(SD.Role_Collaborator));
                    }
                    await _userManager.AddToRoleAsync(applicationUser, FindRole(applicationUser.Role));
                    var userToReturn = await _db.ApplicationUsers.FirstOrDefaultAsync(u => u.UserName == applicationUser.UserName);
                    return Ok(userToReturn);
                } 
             }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception CreateUser:{ex}");
                return BadRequest();
                
            }
                
         }

        
            
        [HttpPut("{id}")]
        public async Task<IActionResult> EditUser(string id, [FromBody]ApplicationUser applicationUser)
        {
            try
            {

                if(id != applicationUser.Id)
                {
                    return BadRequest();
                }

                var user = await _userManager.FindByIdAsync(applicationUser.Id);
                
                if(user == null)
                    return BadRequest();
                else
                {
                    user.Name =  applicationUser.Name;
                    user.City = applicationUser.City;
                    user.StreetAddress = applicationUser.StreetAddress;
                    user.PostalCode = applicationUser.PostalCode;
                    user.State = applicationUser.State;
                    var result = await _userManager.UpdateAsync(user);
                    return Ok(result);
                } 
             }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception CreateUser:{ex}");
                return BadRequest();
                
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                    return Ok();
                else
                    return BadRequest();
            }
            else
               return BadRequest();
        }
    }
}