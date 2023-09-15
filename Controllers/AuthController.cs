using FHSWebServiceApplication.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FHSWebServiceApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Authenticate
        /// </summary>
        /// <param name="login">The login object</param>
        /// <returns>An object with authentication details</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Login
        ///     {
        ///        "username": "admin",
        ///        "password": "admin"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Returns the authentication details with success or failed status</response>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public LoginResult PostLogin([FromBody] LoginModel login)
        {
            if (login.Username == "admin" && login.Password == "admin")
            {
                return new LoginResult
                {
                    Message = "Login successful.",
                    Email = login.Username,
                    Success = true,
                    JwtBearer = CreateJWT(login.Username)
                };
            }

            return new LoginResult { Message = "User/password not found.", Success = false };
        }

        private string CreateJWT(string user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JWT:key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.UniqueName, user),
                    new Claim(JwtRegisteredClaimNames.Sub, user),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}