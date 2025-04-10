using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Perf360.Server.Data.Models;
using Perf360.Server.Dtos;

namespace Perf360.Server.Controllers
{
    [Route("api")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly JwtBearerSettings _jwtBearerSettings;

        public AuthController(UserManager<User> userManager, IOptions<JwtBearerSettings> jwtBearerSettings)
        {
            _userManager = userManager;
            _jwtBearerSettings = jwtBearerSettings.Value;
        }

        [HttpPost("token")]
        public async Task<IActionResult> Login([FromForm] LoginDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.UserName);
            if (user == null)
            {
                return NotFound("User doesn't exist.");
            }

            var result = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!result)
            {
                return BadRequest("UserName or Password mismatch.");
            }
            var roles = await _userManager.GetRolesAsync(user);
            return Ok(new TokenResponse { Token = GenerateToken(user, roles) });
        }

        [Authorize]
        [HttpGet("current-user-information")]
        public async Task<IActionResult> GetCurrentUserInformation()
        {
            var username = HttpContext.User.Identity!.Name!;
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return NotFound("User doesn't exist.");
            }

            var userDto = user.Adapt<UserDto>();
            userDto.Roles = await _userManager.GetRolesAsync(user);

            return Ok(userDto);
        }

        private string GenerateToken(User user, IEnumerable<string> roles)
        {
            var issuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtBearerSettings.IssuerSigningKey));
            var credentials = new SigningCredentials(issuerSigningKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>{
                new (ClaimTypes.Name,user.UserName!),
                new ("uid",user.Id),
            };

            foreach (var role in roles)
            {
                claims.Add(new(ClaimTypes.Role, role));
            }

            var expires = DateTime.Now.AddHours(3);
            var token = new JwtSecurityToken(_jwtBearerSettings.Issuer, _jwtBearerSettings.Audience, claims, expires: expires, signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
