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
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "admin")]
    public class RolesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly RoleManager<Role> _roleManager;

        public RolesController(AppDbContext context, RoleManager<Role> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _context.Roles.ProjectToType<RoleDto>().ToListAsync();
            return Ok(roles);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleDto roleDto)
        {
            var role = roleDto.Adapt<Role>();
            var result = await _roleManager.CreateAsync(role);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(role.Adapt<RoleDto>());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role != null)
            {
                var result = await _roleManager.DeleteAsync(role);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }
            }

            return NoContent();
        }
    }
}
