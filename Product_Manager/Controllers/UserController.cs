using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Product_Manager.Interfaces;
using Product_Manager.Models;
using Product_Manager.Providers;
using Product_Manager.ViewModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using System.Configuration;
using System.Security.Claims;

namespace Product_Manager.Controllers
{

    [ApiController]
    public class UserController : ControllerBase
    {
        
        IUserServices _userServices;
        private UserConext _context;
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
                    if (password == user.Password)
                    {
                        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigurationManager.AppSetting["JWT:Secret"]));
                        var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                        var tokeOptions = new JwtSecurityToken(issuer: ConfigurationManager.AppSetting["JWT:ValidIssuer"], audience: ConfigurationManager.AppSetting["JWT:ValidAudience"], claims: new List<Claim>(), expires: DateTime.Now.AddMinutes(6), signingCredentials: signinCredentials);
                        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                        return Ok(new TokenGen
                        {
                            Token = tokenString
                        });

                    }
                }
            }
            return Ok();
        }

    }
}




