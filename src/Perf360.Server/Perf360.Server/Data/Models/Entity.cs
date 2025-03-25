using System.ComponentModel.DataAnnotations;

namespace Perf360.Server.Data.Models
{
    public class Entity
    {
        [Key]
        public uint ID { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public DateTime? DeleteAt { get; set; }
    }
}
