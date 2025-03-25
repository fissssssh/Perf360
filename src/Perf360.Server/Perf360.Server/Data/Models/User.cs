using Microsoft.AspNetCore.Identity;

namespace Perf360.Server.Data.Models
{
    public class User : IdentityUser
    {
        public virtual HashSet<Review> Reviews { get; set; } = [];
    }
}
