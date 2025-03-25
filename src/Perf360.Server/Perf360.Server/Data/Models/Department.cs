using System.ComponentModel.DataAnnotations;

namespace Perf360.Server.Data.Models
{
    /// <summary>
    /// 部门
    /// </summary>
    public class Department : Entity
    {
        [Required]
        public string Name { get; set; } = default!;
    }
}
