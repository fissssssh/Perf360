using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Perf360.Server.Data;

namespace Perf360.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "admin")]
    public class ReviewDimensionsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ReviewDimensionsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReviewDimension(uint id)
        {
            var reviewDimension = await _context.ReviewDimensions.FindAsync(id);
            if (reviewDimension == null)
            {
                return NotFound();
            }

            reviewDimension.DeleteAt = DateTime.Now;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
