namespace KarpineRfid.API.Models
{
    public class Session
    {
        public int SessionId { get; set; }
        public string SessionName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Tag>? Tags { get; set; }
    }

}
