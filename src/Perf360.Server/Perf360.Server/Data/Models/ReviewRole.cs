using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Perf360.Server.Data.Models.Abstractions;

namespace Perf360.Server.Data.Models;

[Index(nameof(Name), IsUnique = true)]
public class ReviewRole : Entity
{
    [Required]
    public string Name { get; set; } = default!;
}
