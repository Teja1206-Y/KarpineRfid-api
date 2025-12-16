// Models/RfidTag.cs
using System;

namespace KarpineRfid.App.Models
{
    public class RfidTag
    {
        public string Id { get; set; } = string.Empty;      // e.g., EPC or UID
        public int Rssi { get; set; }                       // signal strength
        public DateTime FirstSeen { get; set; } = DateTime.UtcNow;
        public DateTime LastSeen { get; set; } = DateTime.UtcNow;
        public int Count { get; set; } = 1;

        public override string ToString()
            => $"{Id} (RSSI: {Rssi}) x{Count}";
    }
}

