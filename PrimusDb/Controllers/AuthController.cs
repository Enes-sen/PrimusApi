using Business.Abstract;
using Entities.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using Entities.Concrete;
using Microsoft.IdentityModel.Tokens;

namespace PrimusDb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;
        public AuthController(IAuthService authService, IConfiguration configuration)
        {

            _authService = authService;
            _configuration = configuration;

        }
        [HttpPost("register")]
        public IActionResult Register([FromBody] UserRegisterDto userRegisterDto)
        {
            Business.Utilities.Result.IResult userExists = _authService.UserExists(userRegisterDto.UserName);
            if (!userExists.Success)
            {
                return BadRequest(userExists.Message);
            }
            Business.Utilities.Result.IDataResult<User> registerResult = _authService.Register(userRegisterDto);
            return registerResult.Success ? StatusCode(201) : (IActionResult)StatusCode(500);
        }
        [HttpPost("login")]
        public IActionResult Login(UserLoginDto userLoginDto)
        {
            Business.Utilities.Result.IDataResult<User> userToLogin = _authService.Login(userLoginDto);

            if (!userToLogin.Success)
            {
                return BadRequest(userToLogin.Message); // Return the error message instead of userToLogin.Data
            }
            else
            {
                JwtSecurityTokenHandler tokenHandler = new();
                byte[] key = Encoding.ASCII.GetBytes(s: _configuration.GetSection("AppSettings:Secret").ToString());
                User user = userToLogin.Data;

                List<Claim> claims = new()
                {
                    new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
                    new Claim(ClaimTypes.Name, user.username)
                };


                SecurityTokenDescriptor tokenDescriptor = new()
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.Now.AddDays(1), // Use UTC time to avoid timezone issues
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
                };

                SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
                string tokenString = tokenHandler.WriteToken(token);

                return Ok(new { token = tokenString }); // Return the token in a consistent JSON format
            }


        }
    }
}
