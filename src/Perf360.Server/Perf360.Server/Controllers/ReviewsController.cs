using Mapster;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<IActionResult> CreateReview([FromBody] CreateReviewDto reviewDto)
        {

            var userIds = reviewDto.Participants.Select(p => p.UserId).ToList();
            var reviewRoleIds = reviewDto.Participants.SelectMany(p => p.ReviewRoleIds).ToList();

            var users = await _context.Users.Where(u => userIds.Contains(u.Id)).ToDictionaryAsync(u => u.Id, u => u);
            var dimensions = await _context.ReviewDimensions.Where(d => reviewRoleIds.Contains(d.ReviewerRoleId)).ToListAsync();

            using var trascation = await _context.Database.BeginTransactionAsync();

            var review = reviewDto.Adapt<Review>();
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            foreach (var participant in reviewDto.Participants)
            {
                var user = users[participant.UserId];
                foreach (var reviewRoleId in participant.ReviewRoleIds)
                {
                    review.UserReviews.Add(new UserReview { UserId = user.Id, ReviewRoleId = reviewRoleId });
                    await _context.SaveChangesAsync();
                    foreach (var dimension in dimensions.Where(d => d.ReviewerRoleId == reviewRoleId))
                    {
                        var targets = reviewDto.Participants.Where(p => p.ReviewRoleIds.Contains(dimension.TargetRoleId)).ToList();
                        foreach (var target in targets)
                        {
                            if (target.UserId == participant.UserId)
                            {
                                continue; // 自己不能评价自己
                            }
                            var reviewRecord = new ReviewRecord
                            {
                                Name = dimension.Name,
                                Description = dimension.Description,
                                ReviewerId = participant.UserId,
                                TargetId = target.UserId,
                            };
                            review.ReviewRecords.Add(reviewRecord);
                        }
                        await _context.SaveChangesAsync();
                    }
                }
            }

            await trascation.CommitAsync();
            return Ok(review.Adapt<ReviewDto>());
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

        [HttpGet("{id}/review-records")]
        public async Task<IActionResult> GetReviewRecords(uint id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            var reviewRecords = await _context.ReviewRecords.Where(r => r.ReviewId == id).ProjectToType<ReviewRecordDto>().ToListAsync();

            return Ok(reviewRecords);
        }
    }
}
