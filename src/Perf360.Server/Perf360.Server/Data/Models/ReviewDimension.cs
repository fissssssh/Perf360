using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Perf360.Server.Data.Models.Abstractions;

namespace Perf360.Server.Data.Models
{
    /// <summary>
    /// 评价纬度
    /// </summary>
    public class ReviewDimension : Entity
    {
        [Required]
        public string Name { get; set; } = default!;

        public string? Description { get; set; }

        [ForeignKey(nameof(ReviewerRole))]
        public uint ReviewerRoleId { get; set; } = default!;

        [ForeignKey(nameof(TargetRole))]
        public uint TargetRoleId { get; set; } = default!;

        [DeleteBehavior(DeleteBehavior.Cascade)]
        public virtual ReviewRole ReviewerRole { get; set; } = default!;

        [DeleteBehavior(DeleteBehavior.Cascade)]
        public virtual ReviewRole TargetRole { get; set; } = default!;
    }
}
