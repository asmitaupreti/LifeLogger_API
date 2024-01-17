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
using LifeLogger.DataAccess.Data;

namespace LifeLogger.API.Controllers
{
    [Route("api/Auth")]
    [ApiController]
    public class AuthenticationController: ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;
        private string secretKey;
        private APIResponse _response;
        private readonly IMapper _mapper;

        public AuthenticationController(IConfiguration config,
                                        UserManager<ApplicationUser> userManager,
                                        RoleManager<IdentityRole> roleManager,
                                        ApplicationDbContext db,
                                        IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
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

        private async Task<(string accessToken,string role)> GetAccessToken(ApplicationUser user, string jwtTokenId)
        {
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
                    new Claim(JwtRegisteredClaimNames.Jti,jwtTokenId),
                     new Claim(JwtRegisteredClaimNames.Sub,user.Id)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenStr = tokenHandler.WriteToken(token);
            return (tokenStr, role);
          }

        private async Task<string> CreateNewRefreshToken(string userId, string tokenId)
        {
            RefreshToken refreshToken = new()
            {
                IsValid = true,
                UserId = userId,
                JwtTokenId = tokenId,
                ExpiresAt = DateTime.UtcNow.AddMinutes(2),
                Refresh_Token = Guid.NewGuid() + "-" + Guid.NewGuid(),
            };

            await _db.RefreshTokens.AddAsync(refreshToken);
            await _db.SaveChangesAsync();
            return refreshToken.Refresh_Token;
        }

        private void SetJWT(string encrypterToken)
        {

            HttpContext.Response.Cookies.Append("AccessToken", encrypterToken,
                  new CookieOptions
                  {
                      Expires = DateTime.Now.AddDays(15),
                      HttpOnly = true,
                      Secure = true,
                      IsEssential = true,
                      SameSite = SameSiteMode.None
                  });
        }

        private void SetRefreshToken(string encrypterToken)
        {

            HttpContext.Response.Cookies.Append("RefreshToken", encrypterToken,
                 new CookieOptions
                 {
                     Expires = DateTime.Now.AddDays(17),
                     HttpOnly = true,
                     Secure = true,
                     IsEssential = true,
                     SameSite = SameSiteMode.None
                 });

            
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

            var jwtTokenId = $"JIT{Guid.NewGuid()}";
            var userToken = await GetAccessToken(user, jwtTokenId);
            // SetJWT(userToken.accessToken);
            var refreshToken = await CreateNewRefreshToken(user.Id, jwtTokenId);
            // SetRefreshToken(refreshToken);
            LoginResponseDTO loginResponse = new()
            {
              Token = new TokenDTO(){AccessToken =userToken.accessToken, RefreshToken = refreshToken},
                User = _mapper.Map<ApplicationUserResponseDTO>(user)
            };
            loginResponse.User.Role = userToken.role;
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


        [HttpGet("Refresh")]
        public async Task<ActionResult<APIResponse>> GetNewRefreshToken([FromBody] TokenDTO tokenDTO)
        {
            //Find an existing refresh token
            
            var existingRefreshToken = await _db.RefreshTokens.FirstOrDefaultAsync(u => u.Refresh_Token ==tokenDTO.RefreshToken);
            if(existingRefreshToken ==null)
                throw new BadRequestException("Please provide correct data");

            //Compare data from existing refresh and access token provided and if there is any missmatch then consider it as a fraud
            var isTokenValid = GetAccessTokenData(tokenDTO.RefreshToken, existingRefreshToken.UserId, existingRefreshToken.JwtTokenId);
            if(!isTokenValid)
            {
                await MarkTokenAsInvalid(existingRefreshToken);
                throw new BadRequestException("Provided Data is invalid");
            }
                
            
            //When someone tries to use not valid refresh token, fraud possible
            if(!existingRefreshToken.IsValid)
            {
                await MarkAllTokenInChainAsInvalid(existingRefreshToken.UserId, existingRefreshToken.JwtTokenId);

            }

            //If just expires then mark as invalid and return expired
            if(existingRefreshToken.ExpiresAt<DateTime.UtcNow)
            {
                //mark as invalid
                 await MarkTokenAsInvalid(existingRefreshToken);
                throw new BadRequestException("Provided Data is Expired ");
            }

            //replace old refresh with a new one with updated expire date
            var newRefreshToken = await CreateNewRefreshToken(existingRefreshToken.UserId, existingRefreshToken.JwtTokenId);

            SetRefreshToken(newRefreshToken);
            //revoke existing refresh token
            await MarkTokenAsInvalid(existingRefreshToken);

            //generate new access token
            var applicationUser = await _db.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == existingRefreshToken.UserId);
            if(applicationUser==null)
                 throw new BadRequestException("Provided Data is invalid");

            var newAccessToken = await GetAccessToken(applicationUser, existingRefreshToken.JwtTokenId);
            SetJWT(newAccessToken.accessToken);
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = new TokenDTO()
            {
                AccessToken = newAccessToken.accessToken,
                RefreshToken = newRefreshToken
            };

            return _response;

        }

        private bool GetAccessTokenData(string accessToken, string expectedUserId, string expectedTokenId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwt = tokenHandler.ReadJwtToken(accessToken);
            var jwtTokenId = jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Jti).Value;
            var userId = jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value;
            return userId == expectedUserId && jwtTokenId == expectedTokenId;
        }


        private Task MarkTokenAsInvalid(RefreshToken refreshToken)
        {
            refreshToken.IsValid = false;
            return _db.SaveChangesAsync();
        }

        private async Task MarkAllTokenInChainAsInvalid(string userId, string tokenId)
        {
            await _db.RefreshTokens.Where(u => u.UserId == userId
                && u.JwtTokenId == tokenId)
                .ExecuteUpdateAsync(u => u.SetProperty(refeshToken => refeshToken.IsValid, false));
        }
    }
}