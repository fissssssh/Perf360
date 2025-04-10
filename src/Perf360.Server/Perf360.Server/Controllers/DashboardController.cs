using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Perf360.Server.Data;
using Perf360.Server.Data.Models;
using Perf360.Server.Dtos;
using Perf360.Server.Services;

namespace Perf360.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly CurrentUser _currentUser;
        private readonly AppDbContext _context;

        public DashboardController(CurrentUser currentUser, AppDbContext context)
        {
            _currentUser = currentUser;
            _context = context;
        }

        [HttpGet("my-reviews")]
        public async Task<IActionResult> GetMyReviews()
        {
            var reviews = await _context.Reviews.Where(r => r.Participants.Any(u => u.Id == _currentUser.Id)).ProjectToType<ReviewDto>().ToListAsync();
            return Ok(reviews);
        }

        [HttpGet("my-reviews/{id}")]
        public async Task<IActionResult> GetMyReview(uint id)
        {
            var reviewRecords = await _context.ReviewRecords.Where(r => r.ReviewId == id).Where(r => r.ReviewerId == _currentUser.Id).ProjectToType<ReviewRecordDto>().ToListAsync();
            return Ok(reviewRecords);
        }

        [HttpPatch("my-review-records/{id}")]
        public async Task<IActionResult> PatchMyReviewRecord(uint id, [FromBody] JsonPatchDocument<ReviewRecord> patchDoc)
        {
            var allowPaths = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase) { "/score", "/comment" };
            if (!patchDoc.Operations.All(op => allowPaths.Contains(op.path)))
            {
                return BadRequest("Contains illegal fields in patch operations");
            }

            var reviewRecord = await _context.ReviewRecords.Where(r => r.ReviewerId == _currentUser.Id).Where(r => r.Id == id).FirstOrDefaultAsync();
            if (reviewRecord == null)
            {
                return NotFound();
            }

            patchDoc.ApplyTo(reviewRecord);
            await _context.SaveChangesAsync();
            return Ok(reviewRecord.Adapt<ReviewRecordDto>());
        }
    }
}
