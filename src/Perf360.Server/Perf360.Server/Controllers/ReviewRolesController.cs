using System;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Perf360.Server.Data;
using Perf360.Server.Data.Models;
using Perf360.Server.Dtos;

namespace Perf360.Server.Controllers;

[Authorize(Roles = "admin")]
[ApiController]
[Route("api/[controller]")]
public class ReviewRolesController : ControllerBase
{
    private readonly AppDbContext _context;

    public ReviewRolesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetRoles()
    {
        var roles = await _context.ReviewRoles.ProjectToType<ReviewRoleDto>().ToListAsync();
        return Ok(roles);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRole([FromBody] CreateReviewRoleDto roleDto)
    {
        var role = roleDto.Adapt<ReviewRole>();

        await _context.ReviewRoles.AddAsync(role);
        await _context.SaveChangesAsync();

        return Ok(role.Adapt<ReviewRoleDto>());
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRole(string id)
    {
        var role = await _context.ReviewRoles.FindAsync(id);

        if (role != null)
        {
            _context.ReviewRoles.Remove(role);
            await _context.SaveChangesAsync();
        }

        return NoContent();
    }
    [HttpGet("{id}/review-targets")]
    public async Task<IActionResult> GetReviewTargets(uint id)
    {
        var query = _context.ReviewDimensions.Where(x => x.ReviewerRoleId == id);

        var reviewTargets = await query.Select(g => new ReviewRoleDto
        {
            Id = g.TargetRoleId!,
            Name = g.TargetRole.Name,
        }).Distinct().ToListAsync();

        return Ok(reviewTargets);
    }

    [HttpGet("{id}/review-targets/{targetId}/dimensions")]
    public async Task<IActionResult> GetReviewDimensions(uint id, uint targetId)
    {
        var query = _context.ReviewDimensions.Where(x => x.ReviewerRoleId == id).Where(x => x.TargetRoleId == targetId);

        var reviewDimensions = await query.ToListAsync();

        return Ok(reviewDimensions);
    }

    [HttpPost("{id}/review-targets/{targetId}/dimensions")]
    public async Task<IActionResult> CreateReviewDimension(uint id, uint targetId, [FromBody] CreateReviewDimensionDto createReviewDimensionDto)
    {
        var reviewDimension = createReviewDimensionDto.Adapt<ReviewDimension>();
        reviewDimension.ReviewerRoleId = id;
        reviewDimension.TargetRoleId = targetId;

        _context.ReviewDimensions.Add(reviewDimension);
        await _context.SaveChangesAsync();

        return Ok(reviewDimension.Adapt<ReviewDimensionDto>());
    }
}
