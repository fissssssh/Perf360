using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Perf360.Server.Data;
using Perf360.Server.Data.Models;

namespace Perf360.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ReviewsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetReviews()
        {
            return Ok(await _context.Reviews.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> CreateReview([FromBody] Review review)
        {
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            return Ok(review);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(uint id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            review.DeleteAt = DateTime.Now;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("{id}/review_records")]
        public async Task<IActionResult> GetReviewRecords(uint id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            var reviewRecords = await _context.ReviewRecords.Where(r => r.ReviewID == id).ToListAsync();

            return Ok(reviewRecords);
        }

        [Authorize]
        [HttpPost("{id}/review_records")]
        public async Task<IActionResult> CreateReviewRecord(uint id, [FromBody] ReviewRecord reviewRecord)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            reviewRecord.ReviewID = id;
            reviewRecord.ReviewerID = uint.Parse(HttpContext.User.Identity!.Name!);

            await _context.ReviewRecords.AddAsync(reviewRecord);
            await _context.SaveChangesAsync();

            return Ok(reviewRecord);
        }

        [HttpPost("{id}/participants")]
        public async Task<IActionResult> AddParticipants(uint id, string userId)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            review.Participants.Add(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
