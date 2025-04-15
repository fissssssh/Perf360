using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Perf360.Server.Data;
using Perf360.Server.Data.Models;
using Perf360.Server.Dtos;

namespace Perf360.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _context;

        public UsersController(UserManager<User> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var query = from u in _context.Users
                        join ur in _context.UserRoles on u.Id equals ur.UserId into urGroup
                        from ur in urGroup.DefaultIfEmpty()
                        join r in _context.Roles on ur.RoleId equals r.Id into rGroup
                        from r in rGroup.DefaultIfEmpty()
                        group r by u into g
                        select new UserDto
                        {
                            Id = g.Key.Id,
                            UserName = g.Key.UserName,
                            NickName = g.Key.NickName,
                            Email = g.Key.Email,
                            PhoneNumber = g.Key.PhoneNumber,
                            Roles = g.Where(r => r != null).Select(r => r.Name!).ToArray()  // 角色名数组
                        };

            var users = await query.ToListAsync();
            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto userDto)
        {
            var user = userDto.Adapt<User>();
            var result = await _userManager.CreateAsync(user, "12345678");
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(user.Adapt<UserDto>());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("user doesn't exist.");
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return NoContent();
        }

        [HttpGet("{id}/roles")]
        public async Task<IActionResult> GetUserRoles(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("User doesn't exists");
            }

            var roles = await _userManager.GetRolesAsync(user);
            return Ok(roles);
        }

        [HttpPost("{id}/roles")]
        public async Task<IActionResult> EditRoles(string id, [FromBody] string[] roles)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            var retainRoles = currentRoles.Intersect(roles);
            var removeRoles = currentRoles.Except(retainRoles);
            var newRoles = roles.Except(retainRoles);

            var result = await _userManager.AddToRolesAsync(user, newRoles);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            result = await _userManager.RemoveFromRolesAsync(user, removeRoles);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok();
        }
    }
}
