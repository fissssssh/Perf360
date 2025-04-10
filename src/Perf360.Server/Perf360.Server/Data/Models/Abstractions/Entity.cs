using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Perf360.Server.Data.Models.Abstractions
{
    [Index(nameof(DeleteAt))]
    public class Entity
    {
        [Key]
        public uint Id { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }

        [JsonIgnore]
        public DateTime? DeleteAt { get; set; }
    }
}
