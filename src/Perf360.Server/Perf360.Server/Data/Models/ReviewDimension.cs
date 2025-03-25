using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public uint ReviewerRoleID { get; set; }

        [ForeignKey(nameof(TargetRole))]
        public uint TargetRoleID { get; set; }

        public virtual DepartmentRole ReviewerRole { get; set; } = default!;

        public virtual DepartmentRole TargetRole { get; set; } = default!;
    }
}
