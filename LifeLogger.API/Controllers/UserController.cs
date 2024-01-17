using System.Net;
using AutoMapper;
using LifeLogger.DataAccess.Data;
using LifeLogger.Models;
using LifeLogger.Models.DTO;
using LifeLogger.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LifeLogger.API.Middleware.Exceptions;
using Microsoft.AspNetCore.Authorization;


namespace LifeLogger.API.Controllers
{
    [Route("api/User")]
    [ApiController]
    [Authorize(Roles = SD.Role_Admin)]
    public class UserController: Controller
    {
        protected APIResponse _response;
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public UserController(ApplicationDbContext db,UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _response = new();
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
                    
                default: return SD.Role_User;
            }
         }
[Route("api/User/GetAllUser")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetAllUser()
        {
                IEnumerable<ApplicationUser> applicationUserList = await _userManager.Users.ToListAsync();
                foreach (var user in applicationUserList)
                {
                    var role = await _userManager.GetRolesAsync(user);
                    user.Role = role.FirstOrDefault();
                }
                 _response.Result = _mapper.Map<List<ApplicationUserResponseDTO>>(applicationUserList);
                 _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
        }
        internal Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        [Route("api/User/GetCurrentUser")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetCurrentUser()
        {
                ApplicationUser user = await GetCurrentUserAsync();
                user.Role = _userManager.GetRolesAsync(user).GetAwaiter().GetResult().FirstOrDefault();
		           _response.Result = _mapper.Map<ApplicationUserResponseDTO>(user);
                    _response.StatusCode = HttpStatusCode.OK;
                    return Ok(_response);
        }

        [HttpGet("{id}", Name ="GetUser")]
        public async Task<ActionResult<APIResponse>> GetUser(string id)
        {
           
                ApplicationUser user = await _db.ApplicationUsers.Where( u=> u.Id ==id).FirstOrDefaultAsync();
                if(user == null){
                       
                        throw new NotFoundException("User doesnot exist");
                       
                } 
                else{
                    user.Role = _userManager.GetRolesAsync(user).GetAwaiter().GetResult().FirstOrDefault();
                    _response.Result = _mapper.Map<ApplicationUserResponseDTO>(user);
                    _response.StatusCode = HttpStatusCode.OK;
                    return Ok(_response);
                } 
                
            
           
        }

        [HttpPost]
        public async Task<ActionResult<APIResponse>> CreateUser([FromBody]ApplicationUserCreateDTO applicationUserCreateDTO)
        {         
                    var user = await _db.ApplicationUsers.FirstOrDefaultAsync(u => u.UserName == applicationUserCreateDTO.UserName);
                    if(user!= null){
                       throw new BadRequestException("User already exist");
                    }    
                    else
                    {
                        ApplicationUser applicationUser = _mapper.Map<ApplicationUser>(applicationUserCreateDTO);
                        await _userManager.CreateAsync(applicationUser, applicationUserCreateDTO.Password);
                        if(!_roleManager.RoleExistsAsync(SD.Role_Admin).GetAwaiter().GetResult())
                        {
                            await _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin));
                            await _roleManager.CreateAsync(new IdentityRole(SD.Role_User));
                            await _roleManager.CreateAsync(new IdentityRole(SD.Role_Collaborator));
                        }
                        await _userManager.AddToRoleAsync(applicationUser, FindRole(applicationUser.Role));
                        _response.Result = _mapper.Map<ApplicationUserResponseDTO>(applicationUser);
                        _response.StatusCode = HttpStatusCode.Created;
                        return CreatedAtRoute("GetUser", new { id = applicationUser.Id }, _response);
                     } 
                
                
             
                
         }

        
            
        [HttpPut("{id}")]
        public async Task<ActionResult<APIResponse>> EditUser(string id, [FromBody]ApplicationUserUpdateDTO applicationUserUpdateDTO)
        {
            try
            {

                if(applicationUserUpdateDTO == null || id != applicationUserUpdateDTO.Id )
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessage = new List<string>() {"Please make sure you are sending correct data"};
                    
                    return BadRequest(_response);
                }
                else
                {
                    var oldUser = await _userManager.FindByIdAsync(applicationUserUpdateDTO.Id);
                    if(oldUser == null){
                        _response.IsSuccess = false;
                        _response.ErrorMessage = new List<string>() { "User doesnot exist"};
                        return BadRequest(ModelState);
                    }
                    
                    if(applicationUserUpdateDTO.Role != oldUser.Role)
                    {  
                        var OldRoles = await _userManager.GetRolesAsync(oldUser);
                        await _userManager.RemoveFromRolesAsync(oldUser, OldRoles);
                        await _userManager.AddToRoleAsync(oldUser, FindRole(applicationUserUpdateDTO.Role));
                    }
                     await _userManager.UpdateAsync(_mapper.Map<ApplicationUserUpdateDTO, ApplicationUser>(applicationUserUpdateDTO, oldUser));
                    _response.StatusCode = HttpStatusCode.NoContent;
                    _response.IsSuccess = true;
                    return Ok(_response);
                   
                } 
             }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };
                
            }
             return _response;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<APIResponse>> DeleteUser(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user != null)
                {
                    IdentityResult result = await _userManager.DeleteAsync(user);
                    if (result.Succeeded)
                    {
                        _response.StatusCode = HttpStatusCode.NoContent;
                        _response.IsSuccess = true;
                         return Ok(_response);
                    }
                    else
                    {
                        _response.IsSuccess = false;
                        _response.ErrorMessage = new List<string>() {"User couldnot be deleted"};
                        return BadRequest(_response);
                    }                      
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessage = new List<string>() {"User doesnot exist"};
                    return BadRequest(_response);
                }
            }
             catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };
                
            }
             return _response; 
        }
    }
}