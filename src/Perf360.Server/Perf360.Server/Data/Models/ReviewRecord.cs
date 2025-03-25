namespace Perf360.Server.Data.Models
{
    public class ReviewRecord : Entity
    {
        public uint ReviewID { get; set; }

        public uint ReviewerID { get; set; }

        public uint TargetID { get; set; }

        public float Score { get; set; }

        public string? Comment { get; set; }
    }
}
