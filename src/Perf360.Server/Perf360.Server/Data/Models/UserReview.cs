using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Perf360.Server.Data.Models;

public class UserReview
{
    [ForeignKey(nameof(User))]
    public string UserId { get; set; } = default!;

    [ForeignKey(nameof(Review))]
    public uint ReviewId { get; set; } = default!;

    [ForeignKey(nameof(ReviewRole))]
    public uint ReviewRoleId { get; set; } = default;

    public virtual User User { get; set; } = default!;

    public virtual Review Review { get; set; } = default!;

    public virtual ReviewRole ReviewRole { get; set; } = default!;
}
