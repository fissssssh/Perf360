using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Perf360.Server.Data.Models
{
    /// <summary>
    /// 部门角色
    /// </summary>
    public class DepartmentRole : Entity
    {
        [Required]
        public string Name { get; set; } = default!;

        [ForeignKey("Department")]
        public uint? DepartmentID { get; set; }

        public virtual Department? Department { get; set; }
    }
}
