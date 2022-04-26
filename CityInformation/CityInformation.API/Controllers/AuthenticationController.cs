using CityInformation.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CityInformation.API.Controllers
{
    [ApiController(), Route("api/authentication")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public AuthenticationController(IConfiguration configuration) =>
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

        [HttpPost("authenticate")]
        public ActionResult<string> Authenticate([FromBody]AuthenticationRequest body)
        {
            User user = Validate(body.Name, body.Password);

            if (user == null)
            {
                return Unauthorized();
            }

            // Secret as a binary:
            var key = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(_configuration["Authentication:SecretKeyFor"]));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Claims. Identity-related for that user.
            var claimsForToken = new List<Claim>
            {
                new Claim("sub", user.Id.ToString()),
                new Claim("given_name", user.FirstName),
                new Claim("family_name", user.LastName),
                new Claim("city", user.City),
            };

            var dt = DateTime.UtcNow;
            var jwtSecurityToken = new JwtSecurityToken(
                _configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                claimsForToken, dt, dt.AddHours(1), credentials);

            var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            
            return Ok(token);
        }

        /// <summary>
        /// TODO.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private User Validate(string? name, string? password) => 
            new User(1, "ADMIN", "John", "Fedo", "Duluth");
    }
}