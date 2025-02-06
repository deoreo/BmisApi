namespace BmisApi.Logging
{
    public class AuditLogModel
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public int StatusCode {  get; set; }
        public DateTime Timestamp { get; set; }
    }
}
