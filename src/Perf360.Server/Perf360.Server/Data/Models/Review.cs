using Perf360.Server.Data.Models.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Perf360.Server.Data.Models
{
    public class Review : Entity
    {
        [Required]
        public string Name { get; set; } = default!;

        public virtual HashSet<ReviewRecord> ReviewRecords { get; set; } = [];

        public virtual HashSet<UserReview> UserReviews { get; set; } = [];
        public virtual HashSet<User> Participants { get; set; } = [];
    }
}
