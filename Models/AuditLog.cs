namespace HCAMiniEHR.Models
{
    public class AuditLog
    {
        public int Id { get; set; }
        public string TableName { get; set; } = string.Empty;
        public string Operation { get; set; } = string.Empty;
        public int RecordId { get; set; }
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
        public DateTime ChangedAt { get; set; }
        public string ChangedBy { get; set; } = "System";
    }
}
