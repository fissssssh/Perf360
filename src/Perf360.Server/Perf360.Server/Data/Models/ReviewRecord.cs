using Perf360.Server.Data.Models.Abstractions;

namespace Perf360.Server.Data.Models
{
    public class ReviewRecord : Entity
    {
        public uint ReviewId { get; set; }

        public string Name { get; set; } = default!;

        public string? Description { get; set; }

        public string ReviewerId { get; set; } = default!;

        public string TargetId { get; set; } = default!;

        public float? Score { get; set; }

        public string? Comment { get; set; }

        public virtual User Reviewer { get; set; } = default!;

        public virtual User Target { get; set; } = default!;
    }
}
