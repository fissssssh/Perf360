using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Perf360.Server.Data;
using Perf360.Server.Data.Models;

namespace Perf360.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewDimensionsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ReviewDimensionsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetReviewDimensions()
        {
            var reviewDimensions = await _context.ReviewDimensions.ToListAsync();
            return Ok(reviewDimensions);
        }

        [HttpPost]
        public async Task<IActionResult> CreateReviewDimension([FromBody] ReviewDimension reviewDimension)
        {
            _context.ReviewDimensions.Add(reviewDimension);
            await _context.SaveChangesAsync();

            return Ok(reviewDimension);
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
