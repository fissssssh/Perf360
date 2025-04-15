using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Perf360.Server.Data.Models
{
    public class User : IdentityUser
    {
        [Required]
        public string NickName { get; set; } = default!;
        public virtual HashSet<Review> Reviews { get; set; } = [];
        public virtual HashSet<UserReview> UserReviews { get; set; } = [];
    }
}
