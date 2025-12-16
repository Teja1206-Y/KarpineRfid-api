namespace KarpineRfid.API.Models
{
    public class Tag
    {
        public int TagId { get; set; }
        public string EPC { get; set; } = string.Empty;
        public int RSSI { get; set; }
        public int ReadCount { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public int SessionId { get; set; }
        public Session? Session { get; set; }
    }

}
