using System.Text;

using LifeLogger.Models;
using LifeLogger.Models.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using LifeLogger.API.Middleware.Exceptions;
using System.Net;
using LifeLogger.Utility;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace LifeLogger.API.Controllers
{
    [Route("api/Auth")]
    [ApiController]
    public class AuthenticationController: ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private string secretKey;
        private APIResponse _response;
        private readonly IMapper _mapper;

        public AuthenticationController(IConfiguration config,
                                        UserManager<ApplicationUser> userManager,
                                        RoleManager<IdentityRole> roleManager,
                                        IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            secretKey = config.GetValue<string>("ApiSettings:Secret");
            _response = new();
            _mapper = mapper;
            
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

        
       [HttpPost("login")]
        public async Task<ActionResult<APIResponse>> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginRequestDTO.Email);
            if(user == null)
                throw new NotFoundException("User doesnot exist");

            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);
            if (!isValid)
                throw new BadRequestException("Invalid Password");

            //if user was found generate JWT Token

            var role = _userManager.GetRolesAsync(user).GetAwaiter().GetResult().FirstOrDefault();
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName.ToString()),
                    new Claim(ClaimTypes.Role, role),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            LoginResponseDTO loginResponse = new()
            {
                Token = tokenHandler.WriteToken(token),
                User = _mapper.Map<ApplicationUserResponseDTO>(user)
            };
            loginResponse.User.Role = role;
            _response.IsSuccess = true;
            _response.Result = loginResponse;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);

        }

        // [HttpPost("logout")]
        // public async Task<ActionResult<APIResponse>> Logout()
        // {
        //     await _signInManager.SignOutAsync();
        //     return Ok(new APIResponse()
        //     {
        //         IsSuccess = true,
        //         StatusCode = HttpStatusCode.OK,
        //     });
        // }

        [HttpPost("Register")]
        public async Task<ActionResult<APIResponse>> Register([FromBody]ApplicationUserCreateDTO applicationUserCreateDTO)
        {
            var user = await _userManager.Users.AsNoTracking().SingleOrDefaultAsync(u => u.Email == applicationUserCreateDTO.Email);
            if(user!= null)
            {
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
                return Ok(_response);
             } 
                
        }

    }
}