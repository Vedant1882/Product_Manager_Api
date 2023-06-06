
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Product_Manager.Interfaces;
using Product_Manager.Models;
using Product_Manager.ViewModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Product_Manager.Controllers
{

    [ApiController]
    public class UserController : ControllerBase
    {

        IUserServices _userServices;
        UserConext _context;

        public UserController(IUserServices userServices, UserConext context)
        {
            _userServices = userServices;
            _context = context;

        }

        [HttpPost]
        [Route("api/user/register")]
        public async Task<ResponseModel> Register(UserViewModel UsersViewModel)
        {
            return await _userServices.SaveUser(UsersViewModel);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        [Route("api/user/get")]
        public async Task<List<AppUsers>> GetAppUsersList()
        {
            return await _userServices.GetUsers();
        }
        [HttpGet]
        [Route("api/user/getbyid")]
        public async Task<AppUsers> GetUserById(int id)
        {
            return await _userServices.GetUsersById(id);
        }
        [HttpPost]
        [Route("api/user/login")]

        public async Task<IActionResult> Login(UserLoginViewModel loginVm)
        {

            if (loginVm != null)
            {
                AppUsers user;
                user = await _context.AppUsers.FirstOrDefaultAsync(u => u.Email == loginVm.Email && !u.DeletedAt.HasValue);
                if (user != null)
                {
                    var encryption = new HMACSHA512(user.Key);
                    var password = encryption.ComputeHash(Encoding.UTF8.GetBytes(loginVm.Password));
                    var a = user.Password.ToString();
                    var b = password.ToString();
                    if (user.Password.SequenceEqual(password))
                    {
                        var claims = new List<Claim>() { new Claim(ClaimTypes.Name, user.Email),
                        new Claim(ClaimTypes.NameIdentifier, "1") };
                        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.AppSetting["JWT:Secret"]));
                        var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                        var tokeOptions = new JwtSecurityToken(issuer: Configuration.AppSetting["JWT:ValidIssuer"], audience: Configuration.AppSetting["JWT:ValidAudience"], claims, expires: DateTime.Now.AddMinutes(6), signingCredentials: signinCredentials);
                        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                        var xyz = tokenString;
                        return Ok(new TokenGen
                        {
                            Token = tokenString
                        });

                    }
                    else
                    {
                        return Ok(new TokenGen
                        {
                            Token = "",
                        }) ;
                    }
                }
                else
                {

                    return BadRequest();
                }
            }
            return Ok();
        }

    }
}




