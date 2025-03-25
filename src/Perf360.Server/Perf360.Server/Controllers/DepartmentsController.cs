using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Perf360.Server.Data;
using Perf360.Server.Data.Models;

namespace Perf360.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DepartmentsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetDepartments()
        {
            var departments = await _context.Departments.ToListAsync();

            return Ok(departments);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDepartment([FromBody] Department department)
        {
            _context.Departments.Add(department);
            await _context.SaveChangesAsync();

            return Ok(department);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(uint id)
        {
            var department = await _context.Departments.FindAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            department.DeleteAt = DateTime.Now;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("{id}/roles")]
        public async Task<IActionResult> GetDepartmentRoles(uint id)
        {
            var department = await _context.Departments.FindAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            var departmentRoles = await _context.DepartmentRoles.Where(r => r.DepartmentID == id).ToListAsync();

            return Ok(departmentRoles);
        }

        [HttpPost("{id}/roles")]
        public async Task<IActionResult> CreateDepartmentRole(uint id, [FromBody] DepartmentRole departmentRole)
        {
            var department = await _context.Departments.FindAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            departmentRole.DepartmentID = id;
            _context.DepartmentRoles.Add(departmentRole);
            await _context.SaveChangesAsync();

            return Ok(departmentRole);
        }

        [HttpDelete("{id}/roles/{roleId}")]
        public async Task<IActionResult> DeleteDepartmentRole(uint id, uint roleId)
        {
            var departmentRole = await _context.DepartmentRoles.FindAsync(roleId);

            if (departmentRole == null)
            {
                return NotFound();
            }

            departmentRole.DeleteAt = DateTime.Now;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
